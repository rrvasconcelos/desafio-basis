import {ApplicationConfig} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './subjects.route';

export const contactConfig: ApplicationConfig = {
  providers: [provideRouter(routes)]
};
