import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
import { Restservice } from '../../services/restservice/restservice';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NavChartComponent } from '../../common/shared/nav-chart/nav-chart';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { LoaderService } from '../../services/loaderService';
import { AlertService } from '../../common/alert/alert.service';

@Component({
  selector: 'app-mutualfunds',
  imports: [CommonModule, FormsModule, MatTableModule, MatPaginatorModule, MatButtonModule, MatIconModule, NavChartComponent],
  templateUrl: './mutualfunds.html',
  styleUrl: './mutualfunds.scss',
})
export class Mutualfunds {

  constructor(private _restService: Restservice, private cd: ChangeDetectorRef, private loader: LoaderService, private alertService: AlertService) { }
  displayedColumns: string[] = [
    'schemeCode',
    'schemeName',
    'isinGrowth',
    'isinDivReinvestment',
    'action',
    'fav'
  ];
  favSearch$ = new Subject<string>();
  disableAiButton: boolean = false;
  searchText: string = '';
  favsearchText: string = '';
  airesut: any = 'Analysing..!';
  fundData: any;
  favScreen: boolean = false
  startDate: string = '2025-01-01';
  endDate: string = '2026-04-19';
  isViewDetail: boolean = false;
  schemes: any[] = [];
  cachedSchemes: any[] = [];
  favSchemes: any[] = [];
  page: number = 1;
  limit: number = 10;
  currentscheme: any;
  currentUser: any;
  currentUserFav: any = null;
  filteredFavSchemes: any[] = [];
  @ViewChild('aiContainer') aiContainer!: ElementRef;

  ngOnInit() {
    this.loadSchemes();
    this.favSearch$
      .pipe(
        debounceTime(300),
        map(favsearchText => {

          if (!favsearchText || favsearchText.trim() === '') {
            return this.favSchemes; // full list
          }

          return this.favSchemes.filter(x =>
            x.schemeName.toLowerCase().includes(favsearchText.toLowerCase())
          );
        })
      )
      .subscribe(filtered => {

        this.filteredFavSchemes = filtered; // ✅ store filtered list

        this.page = 1; // reset page

        this.updatePagedData(); // ✅ only slice here
      });
  }
  updatePagedData() {
    const startIndex = (this.page - 1) * this.limit;
    const endIndex = startIndex + this.limit;

    this.schemes = this.filteredFavSchemes.slice(startIndex, endIndex);
    this.cd.detectChanges();
  }
  loadSchemes() {
    this.loader.show();
    this._restService.getAllSchemes(this.limit, this.page).subscribe((res: any) => {
      console.log(localStorage.getItem('currentUser'))
      let currentUser: any = localStorage.getItem('currentUser');
      this.currentUser = JSON.parse(currentUser);
      this._restService.GetAllFav(this.currentUser.id).subscribe((f: any) => {
        console.log(f);
        console.log(res)
        this.currentUserFav = f;
        this.cd.detectChanges;
        res.forEach((s: any) => {
          s.isFav = false;
          f.forEach((fav: any) => {
            if (fav.schemeCode == s.schemeCode) {
              s.isFav = true;
            }
          })
        })
        this.schemes = res;
        console.log(this.schemes);
        this.cd.detectChanges();
        this.loader.hide();

      })

    }
    )
  }

  onPageChange(event: PageEvent) {
    if (!this.favScreen) {
      if (this.searchText != null || this.searchText !== '') {
        this.limit = event.pageSize;
        this.page = event.pageIndex + 1; // important
        this.loadSchemes();
      }
    }
    else {
      const startIndex = event.pageIndex * event.pageSize;
      const endIndex = startIndex + event.pageSize;

      this.schemes = this.favSchemes.slice(startIndex, endIndex);
      this.cd.detectChanges;
    }
  }

  viewDetails(scheme: any) {
    this.disableAiButton = false;
    this.fundData = null;
    console.log(scheme);
    this.currentscheme = scheme;
    this.loader.show();
    this._restService.getNavData(scheme.schemeCode, this.startDate, this.endDate).subscribe((s: any) => {
      console.log(JSON.stringify(s));
      this.fundData = s;
      this.isViewDetail = true; this.loader.hide(); this.cd.detectChanges();
    });
  }

  viewLatestDetails(scheme: any) {
    this.disableAiButton = false;
    this.fundData = null;
    this.isViewDetail = true;
    this.cd.detectChanges();
    console.log(scheme);
    this.currentscheme = scheme;
    this.loader.show();
    this._restService.getLatestNavData(scheme.schemeCode).subscribe((s: any) => {
      console.log(s.data)
      this.fundData = s;
      this.loader.hide();
      this.isViewDetail = true; this.cd.detectChanges();
    });

  }

  onDateChange() {
    if (this.startDate && this.endDate) {
      this.viewDetails(this.currentscheme); // you already said you'll handle this
    }
  }



  onSearch() {
    console.log('Search:', this.searchText);
    // call API or filter locally
    if (this.searchText != null && this.searchText != '') {
      this._restService.searchScheme(this.searchText).subscribe((s: any) => {
        this.schemes = s;
        console.log(s);
        this.cd.detectChanges();
      });
    }
  }

  onFavSearch() {
    console.log('Search:', this.searchText);
    // call API or filter locally
    if (this.searchText != null && this.searchText != '') {

    }
  }

  onSearchChange(value: string) {
    this.searchText = value;
    if (!value || value.trim() === '') {
      this.loadSchemes();
      return;
    }
    //    this.onSearch();
  }

  onFavSearchChange(value: string) {
    this.favSearch$.next(value);

    //    this.onSearch();
  }


  analyzeWithAI() {
    this.loader.show()
    this.disableAiButton = true;
    this.cd.detectChanges();
    const data: any = {
      meta: this.fundData.meta,
      data: this.fundData.data.slice(-10) // ✅ last 10
    };
    const prompt = `
              Analyze this mutual fund NAV data and provide:
              - Trend
              - Volatility
              - Risk level
              - Future outlook

              Data:
              ${JSON.stringify(data)}
              `;

    this._restService.getAIAnalysis(prompt).subscribe((s: any) => {
      console.log(s);
      this.airesut = s;
      this.cd.detectChanges()
      setTimeout(() => {
        this.loader.hide()
        this.typeText(this.airesut);

      }, 0);

    });
  }

  typeText(html: string) {
    const element = this.aiContainer.nativeElement;
    element.innerHTML = 'Analysing..!'; // clear

    let i = 0;

    const interval = setInterval(() => {
      element.innerHTML = html.substring(0, i);
      i++;

      if (i > html.length) {
        clearInterval(interval);
      }
    }, 10); // speed (lower = faster)
  }
  toggleYourFav() {
    this.favScreen = !this.favScreen;
    if (this.favScreen) this.openFavorites();
    else this.closeFavourites();
  }

  closeFavourites() {
    this.schemes = this.cachedSchemes;
    this.cd.detectChanges();
  }
  openFavorites() {
    console.log('Open favorites clicked');
    this.GetAllFav();
    // TODO:
    // navigate to favorites page OR toggle favorites view
  }




  toggleFav(item: any) {
    item.isFav = !item.isFav;
    console.log(item);
    let currentUser: any = this.currentUser;
    let favObject: any = {
      "UserId": currentUser["id"],
      "SchemeCode": item.schemeCode,
      "Details": JSON.stringify(item)
    }
    if (item.isFav) {
      this._restService.AddFav(favObject).subscribe((s: any) => {
        console.log(s);
      })
    }
    if (!item.isFav) {
      this._restService.RemoveFav(favObject).subscribe((s: any) => {
        console.log(s);
      })
    }
  }

  GetAllFav() {
    let currentUser: any = this.currentUser;
    this._restService.GetAllFav(currentUser["id"]).subscribe((s: any) => {
      console.log(s);
      this.favSchemes = s.map((x: any) => JSON.parse(x.details));
      console.log(this.favSchemes);
      this.cachedSchemes = this.schemes;
      const startIndex = (this.page - 1) * this.limit;
      const endIndex = startIndex + this.limit;
      this.cd.detectChanges;
      this.schemes = this.favSchemes.slice(startIndex, endIndex);
      this.cd.detectChanges();
    })

  }


  mailFavourites() {
    let currentUser: any = this.currentUser;
    const request = {
      userId: currentUser.id,
      schemes: this.schemes
    };
    if (this.schemes.length == 0) {
       this.alertService.show("There are currently no Favourties!");
       this.cd.detectChanges();
    }
    else {
      this._restService.MailFav(request).subscribe((res: any) => {
        console.log(res);
        if (res != null) {
          this.alertService.show("Mail sent successfully!");
        }
        else {
          this.alertService.show("Unable to Mail!Please try again later");
        }
        this.cd.detectChanges();
      })
    };


  }
}
