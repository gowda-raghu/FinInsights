using Microsoft.EntityFrameworkCore;
using Test.DataLayer;
using Test.DataModels;
public class MfService : IMfService
{

    private readonly HttpClient _httpClient;
    private readonly RaghuDbContext _context;

    private readonly string baseUrl = "https://api.mfapi.in/";
    private readonly IMfService _mfService;
    private readonly IEmailClient _emailclient;

    private readonly IUserService _userService;
    public MfService(HttpClient httpClient, RaghuDbContext context, IEmailClient emailClient, IUserService userService)
    {
        _httpClient = httpClient;
        _context = context;
        _emailclient = emailClient;
        _userService = userService;
    }


    public async Task<List<Scheme>> GetSchemes(int limit, int page)
    {
        int offset = (page - 1) * limit;

        var url = $"{baseUrl}mf?limit={limit}&offset={offset}";
        Console.WriteLine(url);

        var schemes = await _httpClient.GetFromJsonAsync<List<Scheme>>(url);

        return schemes ?? new List<Scheme>();
    }


    public async Task<FundResponse> GetNavData(int schemecode, string startDate, string endDate)
    {
        var url = $"{baseUrl}mf/{schemecode}?startDate={startDate}&endDate={endDate}";
        Console.WriteLine(url);

        var fundResponse = await _httpClient.GetFromJsonAsync<FundResponse>(url);

        return fundResponse ?? null;
    }

    public async Task<FundResponse> GetLatestNavData(int schemecode)
    {
        var url = $"{baseUrl}mf/{schemecode}/latest";
        Console.WriteLine(url);

        var fundResponse = await _httpClient.GetFromJsonAsync<FundResponse>(url);

        return fundResponse ?? null;
    }


    public async Task<List<Scheme>> SearchSchema(string searchinput)
    {
        var url = $"{baseUrl}mf/search?q={searchinput}";
        Console.WriteLine(url);

        var schemes = await _httpClient.GetFromJsonAsync<List<Scheme>>(url);

        return schemes ?? null;
    }

    public async Task<int> AddFav(FavouriteDto favourite)
    {
        Favourite f = new Favourite
        {
            UserId = favourite.UserId,
            Details = favourite.Details,
            SchemeCode = favourite.SchemeCode
        };
        _context.Favourites!.Add(f);
        await _context.SaveChangesAsync();
        return favourite.SchemeCode;

    }

    public async Task<int> RemoveFav(FavouriteDto favourite)
    {
        var existing = await _context.Favourites
            .FirstOrDefaultAsync(f =>
                f.UserId == favourite.UserId &&
                f.SchemeCode == favourite.SchemeCode);

        if (existing == null)
        {
            return -1; // not found
        }

        _context.Favourites.Remove(existing);
        await _context.SaveChangesAsync();

        return favourite.SchemeCode;
    }

    public async Task<List<Favourite>> GetAllFav(Guid userId)
    {
        List<Favourite> f = await _context.Favourites!.Where(x => x.UserId == userId).ToListAsync();
        return f;
    }

    public async Task<MailFavDto> MailFav(MailFavDto dto)
    {
        var user = await _userService.GetUserByID(dto.UserId);
        var htmlBody = MfService.ConstructFavTemplate(
            user.Username,
            dto.Schemes);
        var favmail = new EmailRequestDto
        {
            To = user.Email,
            Subject = $"Your Favourite Mutual Fund Schemes - {DateTime.Now:dd MMM yyyy}",
            Body = htmlBody
        };
        var isMailsent = await _emailclient.SendMailAsync(favmail);
        if (isMailsent)
        {
            return dto;
        }
        return null;

    }


    private static string ConstructFavTemplate(
        string userName,
        List<FavScheme> schemes)
    {
        var rows = string.Join("",
            schemes.Select(s => $@"
                <tr>
                    <td>{s.SchemeCode}</td>
                    <td>{s.SchemeName}</td>
                </tr>"));

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            color: #333;
        }}

        .container {{
            max-width: 800px;
            margin: auto;
            padding: 20px;
        }}

        .header {{
            background-color: #6C63FF;
            color: white;
            padding: 15px;
            border-radius: 8px;
            text-align: center;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }}

        th {{
            background-color: #6C63FF;
            color: white;
            padding: 12px;
            text-align: left;
        }}

        td {{
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }}

        tr:nth-child(even) {{
            background-color: #f7f7f7;
        }}

        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #666;
        }}
    </style>
</head>
<body>
    <div class='container'>

        <div class='header'>
            <h2>Your Favourite Mutual Fund Schemes</h2>
        </div>

        <p>Hello {userName},</p>

        <p>
            Please find below your favourite mutual fund schemes as of
            <strong>{DateTime.Now:dd MMM yyyy}</strong>.
        </p>

        <table>
            <thead>
                <tr>
                    <th>Scheme Code</th>
                    <th>Scheme Name</th>
                </tr>
            </thead>
            <tbody>
                {rows}
            </tbody>
        </table>

        <div class='footer'>
            <p>
                Generated automatically by FinInsight.
            </p>
        </div>

    </div>
</body>
</html>";
    }

}