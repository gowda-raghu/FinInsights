import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './services/authservice';
import { HighchartsChartComponent } from 'highcharts-angular';
import { importProvidersFrom } from '@angular/core';

// bootstrapApplication(App, appConfig)
//   .catch((err) => console.error(err));

bootstrapApplication(App, {
  providers: [provideRouter(routes),provideHttpClient(
      withInterceptors([authInterceptor])
    ),importProvidersFrom(HighchartsChartComponent) ]
}).catch(err => console.error(err));