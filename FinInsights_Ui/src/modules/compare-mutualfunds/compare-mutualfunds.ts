import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { PathService } from '../../common/services/path.service';
import { LoaderService } from '../../services/loaderService';
import { AlertService } from '../../common/alert/alert.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Restservice } from '../../services/restservice/restservice';
import { forkJoin } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NavChartComponent } from '../../common/shared/nav-chart/nav-chart';
@Component({
  selector: 'app-compare-mutualfunds',
  imports: [CommonModule, FormsModule, MatFormFieldModule,
    MatSelectModule, NavChartComponent, MatButtonModule],
  templateUrl: './compare-mutualfunds.html',
  styleUrl: './compare-mutualfunds.scss',
})
export class CompareMutualfunds {
  currentUser: any;
  fund1Data: any;
  startDate: string = '2025-01-01';
  endDate: string = '2026-04-19';
  fund2Data: any;
  comparisonChartData: any = [];
  compareAiResult = '';

  isAiLoading = false;

  disableCompareAiButton = true;

  constructor(private _restService: Restservice, private router: Router, private pathService: PathService, private cd: ChangeDetectorRef, private loader: LoaderService, private alertService: AlertService) { }

  selectedFund1: any = null;
  selectedFund2: any = null;
  favourites: any = [];

  ngOnInit() {
    this.GetAllFav();
  }

  GetAllFav() {
    let currentUser: any = localStorage.getItem('currentUser');
    this.currentUser = JSON.parse(currentUser);
    currentUser = this.currentUser;
    this._restService.GetAllFav(currentUser["id"]).subscribe((s: any) => {
      console.log(s);
      this.favourites = s.map((x: any) => JSON.parse(x.details));
      if (this.favourites.length < 2) {
        this.alertService.show("Please add at least two funds to your favourites to compare them.");
      }
      console.log(this.favourites);
      this.cd.detectChanges();
    })

  }

  get fund1Options() {
    return this.favourites.filter(
      (x: any) => x.schemeCode !== this.selectedFund2?.schemeCode
    );
  }

  get fund2Options() {
    return this.favourites.filter(
      (x: any) => x.schemeCode !== this.selectedFund1?.schemeCode
    );
  }

  buildComparisonChartData() {

    const fund1 = [...this.fund1Data.data].reverse();
    const fund2 = [...this.fund2Data.data].reverse();

    const count = Math.min(
      fund1.length,
      fund2.length
    );

    const baseFund1 = Number(fund1[0].nav);
    const baseFund2 = Number(fund2[0].nav);

    this.comparisonChartData = [];
    this.cd.detectChanges();
    for (let i = 0; i < count; i++) {

      this.comparisonChartData.push({

        date: fund1[i].date,

        fund1:
          +(
            (Number(fund1[i].nav) / baseFund1) * 100
          ).toFixed(2),

        fund2:
          +(
            (Number(fund2[i].nav) / baseFund2) * 100
          ).toFixed(2)

      });

    }
    this.cd.detectChanges();
  }

  compareFunds() {
    this.cd.detectChanges();
    this.loader.show();

    forkJoin({
      fund1: this._restService.getNavData(this.selectedFund1.schemeCode, this.startDate, this.endDate),
      fund2: this._restService.getNavData(this.selectedFund2.schemeCode, this.startDate, this.endDate)
    }).subscribe({
      next: (result: any) => {

        console.log(result.fund1);
        console.log(result.fund2);

        this.fund1Data = result.fund1;
        this.fund2Data = result.fund2;
        this.disableCompareAiButton = false;
        this.cd.detectChanges();

        if (this.fund1Data.data.length == 0 || this.fund2Data.data.length == 0) {
          let message = '';

          if (this.fund1Data?.data?.length === 0 &&
            this.fund2Data?.data?.length === 0) {

            message =
              'No NAV data is available for either of the selected funds.';

          }
          else if (this.fund1Data?.data?.length === 0) {

            message =
              `${this.selectedFund1.schemeName} does not have NAV data available for comparison.`;

          }
          else if (this.fund2Data?.data?.length === 0) {

            message =
              `${this.selectedFund2.schemeName} does not have NAV data available for comparison.`;

          }
          this.alertService.show(message);
        }
        else {

          this.buildComparisonChartData();
          this.buildComparisonMetrics();
          this.cd.detectChanges();
          //this.GetAIInsights();
        }

        this.loader.hide();
      },
      error: err => {
        console.error(err);
        this.loader.hide();
      }
    });

  }

  fund1Return = 0;
  fund2Return = 0;

  winnerFundName = '';
  winnerFundReturn = 0;

  calculateReturn(data: any[]): number {

    if (!data || data.length < 2)
      return 0;

    const latest = Number(data[0].nav);

    const oldest = Number(data[data.length - 1].nav);

    return Number(
      (((latest - oldest) / oldest) * 100)
        .toFixed(2)
    );
  }

  buildComparisonMetrics() {

    this.fund1Return =
      this.calculateReturn(
        this.fund1Data.data
      );

    this.fund2Return =
      this.calculateReturn(
        this.fund2Data.data
      );

    if (this.fund1Return > this.fund2Return) {

      this.winnerFundName =
        this.fund1Data.meta.scheme_name;

      this.winnerFundReturn =
        this.fund1Return;

    }
    else {

      this.winnerFundName =
        this.fund2Data.meta.scheme_name;

      this.winnerFundReturn =
        this.fund2Return;
    }
  }


  GetAIInsights() {
    const comparisonData = {
      fund1: {
        schemeName: this.fund1Data.meta.scheme_name,
        fundHouse: this.fund1Data.meta.fund_house,
        category: this.fund1Data.meta.scheme_category,
        type: this.fund1Data.meta.scheme_type,
        latestNav: this.fund1Data.data[0]?.nav,
        latestNavDate: this.fund1Data.data[0]?.date,
        returnPercentage: this.fund1Return,
        totalRecords: this.fund1Data.data.length
      },

      fund2: {
        schemeName: this.fund2Data.meta.scheme_name,
        fundHouse: this.fund2Data.meta.fund_house,
        category: this.fund2Data.meta.scheme_category,
        type: this.fund2Data.meta.scheme_type,
        latestNav: this.fund2Data.data[0]?.nav,
        latestNavDate: this.fund2Data.data[0]?.date,
        returnPercentage: this.fund2Return,
        totalRecords: this.fund2Data.data.length
      },

      winner: {
        schemeName: this.winnerFundName,
        returnPercentage: this.winnerFundReturn
      }
    };
    this.isAiLoading = true;

    this.disableCompareAiButton = true;

    this.compareAiResult = '';
    this._restService.getCompareFundAIanalysis(comparisonData).subscribe((response: any) => {
      console.log(response);

      this.compareAiResult = response;

      this.isAiLoading = false;
      this.cd.detectChanges()

    });
  }


}
