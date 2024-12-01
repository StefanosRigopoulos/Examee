import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_essentials/services/account.service';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { User } from '../_essentials/models/user';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavBarComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {}

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}