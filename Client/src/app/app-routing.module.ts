import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UploadComponent } from './upload/upload.component';
import { NotFoundComponent } from './_essentials/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_essentials/errors/server-error/server-error.component';
import { AdminComponent } from './admin/admin.component';
import { MemberProfileComponent } from './member/member-profile/member-profile.component';
import { AuthGuard } from './_essentials/guards/auth.guard';
import { AdminGuard } from './_essentials/guards/admin.guard';
import { MemberDetailResolver } from './_essentials/resolvers/member-details.resolver';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'upload', component: UploadComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]},
      {path: ':username', component: MemberProfileComponent, resolve: {member: MemberDetailResolver}},
    ]
  },
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
