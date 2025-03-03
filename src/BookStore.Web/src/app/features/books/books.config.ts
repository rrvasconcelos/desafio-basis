import {ApplicationConfig} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './books.routes';

export const contactConfig: ApplicationConfig = {
  providers: [provideRouter(routes)]
};
