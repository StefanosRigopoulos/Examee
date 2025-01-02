import { Routes } from "@angular/router";
import { NotFoundComponent } from "./_essentials/errors/not-found/not-found.component";
import { ServerErrorComponent } from "./_essentials/errors/server-error/server-error.component";
import { AdminComponent } from "./admin/admin.component";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { MemberProfileComponent } from "./member/member-profile/member-profile.component";
import { RegisterComponent } from "./register/register.component";
import { UploadComponent } from "./upload/upload.component";
import { MemberDetailResolver } from "./_essentials/resolvers/member-details.resolver";
import { AdminGuard } from "./_essentials/guards/admin.guard";
import { AuthGuard } from "./_essentials/guards/auth.guard";
import { PreventExitGuard } from "./_essentials/guards/prevent-exit.guard";

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'upload', component: UploadComponent, canDeactivate: [PreventExitGuard]},
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