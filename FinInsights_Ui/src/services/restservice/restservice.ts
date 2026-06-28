import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-restservice',
  imports: [],
  template: '<p></p>',
  styleUrls: [],
  standalone: true
})
@Injectable({
  providedIn: 'root'
})
export class Restservice {

  private baseUrl = environment.apiUrl; // your .NET API
  constructor(private https: HttpClient) { }

  login(credentials: any) {
    const params = new HttpParams()
      .set('username', credentials.username)
      .set('password', credentials.password);
    // return this.https.post(`${this.baseUrl}/v2/Login/login`, null, {
    //   params,
    //   //withCredentials: true
    // });
    return this.https.post(`${this.baseUrl}/v2/Login/login`, {
      'username': credentials.username,
      'password': credentials.password
    }, {
      params,
      //withCredentials: true
    }).pipe(
      catchError((error) => { return throwError(() => error); }));;
  }

  AddUser(userdetails: any): Observable<any> {
    return this.https.post(`${this.baseUrl}/v2/Login/AddUser`, userdetails, {
      //withCredentials: true
    }).pipe(
      catchError((error) => { return throwError(() => error); }));;;
  }

  getAllSchemes(limit: number, page: number): Observable<any> {

    const params = new HttpParams()
      .set('limit', limit)
      .set('page', page);

    // return this.https.get(`${this.baseUrl}/v1/Mf/GetSchemes`, {
    //   params,
    //   withCredentials: true
    // }).pipe(
    //   catchError((error) => throwError(() => error))
    // );

    // return this.https.get(`${this.baseUrl}/v1/Mf/GetSchemes`, {
    //   params,
    //   headers: {
    //     Authorization: `Bearer ${localStorage.getItem('token')}`
    //   }
    // })
    return this.https.get(`${this.baseUrl}/v1/Mf/GetSchemes`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );
  }


  getNavData(schemeCode: any, startDate: string, endDate: string) {
    const params = new HttpParams()
      .set('schemeCode', schemeCode.toString())
      .set('startDate', startDate)
      .set('endDate', endDate);
    console.log(schemeCode);
    return this.https.get(`${this.baseUrl}/v1/Mf/GetNavData`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );

  }

  getLatestNavData(schemeCode: any) {
    const params = new HttpParams()
      .set('schemeCode', schemeCode.toString());
    return this.https.get(`${this.baseUrl}/v1/Mf/GetLatestNavData`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );

  }

  searchScheme(searchinput: string) {
    const params = new HttpParams()
      .set('searchinput', searchinput.toString());
    return this.https.get(`${this.baseUrl}/v1/Mf/Search`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );
  }

  getAIAnalysis(prompt: string) {
    const params = new HttpParams().set('prompt', prompt);

    return this.https.post(
      `${this.baseUrl}/AIChat/Chat`,
      null, // ✅ no body
      { params, responseType: 'text' }
    ).pipe(
      catchError((error) => throwError(() => error))
    );
  }

  getCompareFundAIanalysis(comparisonData: any) {
    return this.https.post(
      `${this.baseUrl}/AIChat/CompareFundsAIAnalysis`,
      comparisonData, // ✅ no body
      {
        responseType: 'text'
      }
    ).pipe(
      catchError((error) => throwError(() => error))
    );
  }


  AddFav(favObject: any) {
    return this.https.put(
      `${this.baseUrl}/v1/Mf/AddFav`,
      favObject
    ).pipe(
      catchError((error) => throwError(() => error))
    );
  }

  RemoveFav(favObject: any) {
    return this.https.put(
      `${this.baseUrl}/v1/Mf/RemoveFav`,
      favObject
    ).pipe(
      catchError((error) => throwError(() => error))
    );
  }

  GetAllFav(userid: any) {
    const params = new HttpParams().set('userId', userid);
    return this.https.get(
      `${this.baseUrl}/v1/Mf/GetAllFav`, {
      params
    }
    ).pipe(
      catchError((error) => throwError(() => error))
    );
  }


  MailFav(request: any) {
    return this.https.post(
      `${this.baseUrl}/v1/Mf/MailFav`,
      request
    ).pipe(
      catchError((error) => throwError(() => error))
    );

  }



  getUsers(): Observable<any> {
    return this.https.get(`${this.baseUrl}/Login/GetAllUser`, {
      withCredentials: true
    });
  }


  getUserById(id: Number): Observable<any> {
    return this.https.get(`${this.baseUrl}/Login/GetUserById/${id}`, {
      withCredentials: true
    });
  }


  getCountryLists(): Observable<any> {
    return this.https.get(
      `${this.baseUrl}/v1/News/GetCountryList`
    );
  }

  getLatestNews(countrycode: string): Observable<any> {
    const params = new HttpParams().set('countryCode', countrycode);
    return this.https.get(`${this.baseUrl}/v1/News/GetLatestNews`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );
  }


  //stocks
  searchStocks(searchInput: string): Observable<any> {
    const params = new HttpParams().set('searchinput', searchInput);
    return this.https.get(`${this.baseUrl}/v1/Stocks/Search`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );
  }


  //StockDetails
  GetStockDetails(symbol: string): Observable<any> {
    const params = new HttpParams().set('symbol', symbol);
    return this.https.get(`${this.baseUrl}/v1/Stocks/StockDetails`, {
      params
    }).pipe(
      catchError((error) => throwError(() => error))
    );
  }
}
