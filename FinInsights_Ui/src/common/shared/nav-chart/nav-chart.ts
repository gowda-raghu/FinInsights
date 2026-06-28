import { Component, Input, OnChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { HighchartsChartComponent } from 'highcharts-angular';
import { HighchartsChartModule } from 'highcharts-angular';
@Component({
  selector: 'app-nav-chart',
  standalone: true,
  imports: [HighchartsChartModule],
  templateUrl: './nav-chart.html',
  styleUrls: ['./nav-chart.scss']
})
export class NavChartComponent {

  @Input() navData: any[] = [];
  @Input() type: string = "default";
  @Input() fund1Name: string = ""
  @Input() fund2Name: string = ""
  isDark:boolean=false;
  textcolor:string='#fff';

  Highcharts: typeof Highcharts = Highcharts;
  chartOptions: Highcharts.Options = {};

  ngOnInit() {
    console.log(this.navData);
    const savedTheme = localStorage.getItem('theme');

    this.isDark = savedTheme !== 'light';
    this.textcolor=this.isDark?'#fff':'#000';
    if (!this.navData || this.navData.length === 0) return;

    if (this.type == "compare") {
      const categories = this.navData.map(x => x.date);

      const fund1Values = this.navData.map(
        x => Number(x.fund1)
      );

      const fund2Values = this.navData.map(
        x => Number(x.fund2)
      );

      this.chartOptions = {
        chart: {
          type: 'line',
          backgroundColor: 'transparent'
        },

        title: {
          text: 'Growth of ₹100 Invested',
          style: {
            color: this.textcolor
          }
        },

        xAxis: {
          categories,
          labels: {
            style: {
              color: this.textcolor
            }
          }
        },

        yAxis: {
          title: {
            text: 'Investment Value',
            style: {
              color: this.textcolor
            }
          },
          labels: {
            style: {
              color: this.textcolor
            }
          }
        },

        series: [
          {
            name: this.fund1Name,
            type: 'line',
            data: fund1Values,
            color: '#f6e25c'
          },
          {
            name: this.fund2Name,
            type: 'line',
            data: fund2Values,
            color: '#c52222'
          }
        ],

        legend: {
          itemStyle: {
            color: this.textcolor
          }
        },

        tooltip: {
          shared: true,
          valueDecimals: 2
        },

        credits: {
          enabled: false
        }
      };
    }
    else {
      const categories = this.navData.map(x => x.date).reverse();
      const values = this.navData.map(x => Number(x.nav)).reverse();
      console.log(categories, values)
      this.chartOptions = {
        chart: {
          type: 'line',
          backgroundColor: 'transparent'
        },
        title: {
          text: 'NAV Trend',
          style: { color: this.textcolor }
        },
        xAxis: {
          categories,
          labels: { style: { color: this.textcolor } }
        },
        yAxis: {
          title: { text: 'NAV', style: { color: this.textcolor } },
          labels: { style: { color: this.textcolor } }
        },
        series: [{
          name: 'NAV',
          type: 'line',
          data: values,
          color: this.textcolor
        }],
        legend: {
          itemStyle: { color: this.textcolor }
        },
        credits: { enabled: false }
      };
    }

  }
}