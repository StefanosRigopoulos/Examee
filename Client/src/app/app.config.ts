import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { ApplicationConfig, importProvidersFrom } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule } from "@angular/material/dialog";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { BrowserModule } from "@angular/platform-browser";
import { provideAnimations } from "@angular/platform-browser/animations";
import { provideRouter } from "@angular/router";
import { ModalModule } from "ngx-bootstrap/modal";
import { NgxSpinnerModule } from "ngx-spinner";
import { ErrorInterceptor } from "./_essentials/interceptors/error.interceptor";
import { JwtInterceptor } from "./_essentials/interceptors/jwt.interceptor";
import { LoadingInterceptor } from "./_essentials/interceptors/loading.interceptor";
import { routes } from "./app.routes";

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(routes),
        importProvidersFrom(BrowserModule,
                            FormsModule,
                            ReactiveFormsModule,
                            MatButtonModule,
                            MatProgressBarModule,
                            MatIconModule,
                            MatSnackBarModule,
                            MatDialogModule,
                            NgxSpinnerModule,
                            ModalModule.forRoot(),
                            NgxSpinnerModule.forRoot({ type: 'clockwise' })
                        ),
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi()),
        provideAnimations()
    ]
}