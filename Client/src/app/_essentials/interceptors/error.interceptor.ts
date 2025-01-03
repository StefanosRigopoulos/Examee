import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ErrorModalComponent } from '../errors/error-modal/error-modal.component';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private dialog: MatDialog) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const modelStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              } else {
                this.openErrorModal(error);
              }
              break;
            case 401:
              this.openErrorModal(error);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}}
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.openErrorModal(error);
              console.log(error);
          }
        }
        throw error;
      })
    )
  }

  private openErrorModal(error: HttpErrorResponse): void {
    this.dialog.open(ErrorModalComponent, {
      width: '400px',
      data: {
        title: 'Error Notification',
        status: error.status,
        message: error.error.message || 'An unexpected error occurred',
        details: error.error.details || 'No further details are provided',
      },
      disableClose: true,
    });
  }
}
