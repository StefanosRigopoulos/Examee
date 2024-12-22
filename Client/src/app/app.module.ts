import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtInterceptor } from './_essentials/interceptors/jwt.interceptor';
import { NgxSpinnerModule } from "ngx-spinner";
import { HasRoleDirective } from './_essentials/directives/has-role.directive';
import { ErrorInterceptor } from './_essentials/interceptors/error.interceptor';
import { LoadingInterceptor } from './_essentials/interceptors/loading.interceptor';
import { ModalModule } from 'ngx-bootstrap/modal';

import { TextInputComponent } from './_essentials/forms/text-input/text-input.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { NavBarComponent } from './navbar/navbar.component';
import { UploadComponent } from './upload/upload.component';
import { FooterComponent } from './footer/footer.component';
import { NotFoundComponent } from './_essentials/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_essentials/errors/server-error/server-error.component';
import { AdminComponent } from './admin/admin.component';
import { MemberProfileComponent } from './member/member-profile/member-profile.component';
import { MemberExamItemComponent } from './member/member-exam-item/member-exam-item.component';
import { ErrorModalComponent } from './_essentials/errors/error-modal/error-modal.component';

// Upload Template
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    TextInputComponent,
    NavBarComponent,
    UploadComponent,
    FooterComponent,
    HasRoleDirective,
    NotFoundComponent,
    ServerErrorComponent,
    AdminComponent,
    MemberProfileComponent,
    MemberExamItemComponent,
    ErrorModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatProgressBarModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    NgxSpinnerModule,
    ModalModule,
    NgxSpinnerModule.forRoot({
      type: 'clockwise'
    })
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
