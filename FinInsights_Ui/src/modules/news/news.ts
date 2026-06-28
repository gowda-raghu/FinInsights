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
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
@Component({
  selector: 'app-news',
  imports: [
    CommonModule,
    FormsModule,
    MatSelectModule,
    MatCardModule,
    MatChipsModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './news.html',
  styleUrl: './news.scss',
})
export class News {


  constructor(private _restService: Restservice, private cd: ChangeDetectorRef, private loader: LoaderService) { }
  expandedNewsId: string | null = null;

  countryList: any = [];
  latestnewslist: any = [];
  selectedCountryCode: string = 'in';
  ngOnInit() {
    this.loader.show();
    this._restService.getCountryLists().subscribe((c: any) => {
      console.log(c);
      this.countryList = c;
      this.getLatestNews();
    })

  }
 
  getLatestNews(){
    this._restService.getLatestNews(this.selectedCountryCode).subscribe((n: any) => {
        console.log(n);
        this.latestnewslist=n?.news;
        this.cd.detectChanges();
        this.loader.hide();
      })
  }


  toggleNews(id: string) {
    this.expandedNewsId =
      this.expandedNewsId === id
        ? null
        : id;
  }

  onCountryChange() {
    this.loader.show();
    console.log(this.selectedCountryCode);
    this.getLatestNews();
  }
}
