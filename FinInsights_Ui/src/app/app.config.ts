import { ApplicationConfig, inject, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from '../services/authservice';
import { finalize } from 'rxjs';
import { LoaderService } from '../services/loaderService';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    provideHttpClient(),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          const loader = inject(LoaderService);
          loader.show();

          return next(req).pipe(
            finalize(() => loader.hide())
          );
        }
      ])
    )
  ]
};
