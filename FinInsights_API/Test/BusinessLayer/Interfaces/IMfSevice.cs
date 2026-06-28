using Test.DataModels;

public interface IMfService
{
    Task<List<Scheme>> GetSchemes(int limit, int page);

    Task<FundResponse> GetNavData(int schemacode, string startDate, string endDate);

    Task<FundResponse> GetLatestNavData(int schemecode);

    Task<List<Scheme>> SearchSchema(string searchinput);

    Task<int> AddFav(FavouriteDto favourite);

    Task<int> RemoveFav(FavouriteDto favourite);

    Task<List<Favourite>> GetAllFav(Guid userId);

    Task<MailFavDto> MailFav(MailFavDto dto);

}