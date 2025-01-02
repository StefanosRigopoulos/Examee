import { Component, OnInit } from '@angular/core';
import { User } from './_essentials/models/user';
import { AccountService } from './_essentials/services/account.service';
import { FooterComponent } from './footer/footer.component';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './navbar/navbar.component';
import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    imports: [NgxSpinnerComponent, NavBarComponent, RouterOutlet, FooterComponent]
})
export class AppComponent implements OnInit {
  title = 'Examee';

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user')!);
    this.accountService.setCurrentUser(user);
  }
}
