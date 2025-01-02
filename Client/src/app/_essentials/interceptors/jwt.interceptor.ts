import { inject, Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private accountService = inject(AccountService);

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this.accountService.currentUser()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.accountService.currentUser()?.token}`
        }
      })
    }
    return next.handle(request);
  }
}