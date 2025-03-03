import {HttpHandlerFn, HttpRequest} from '@angular/common/http';
import { NgxSpinnerService } from "ngx-spinner";
import {inject} from '@angular/core';
import {catchError, finalize, tap} from 'rxjs';

export function loadingInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const spinner = inject(NgxSpinnerService);

  spinner.show();

  return next(req).pipe(
    finalize(() => {
      spinner.hide();
    })
  );
}
