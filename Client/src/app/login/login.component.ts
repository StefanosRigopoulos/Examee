import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../_essentials/services/account.service';
import { MemberService } from '../_essentials/services/member.service';
import { Member } from '../_essentials/models/member';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  members: Member[] = [];

  constructor(public accountService: AccountService, private memberService: MemberService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]]
    });
  }

  submit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: response => console.log('Successful login!'),
      error: error => console.log()
    })
  }

  showUsers(){
    this.memberService.getMembers().subscribe({
      next: response => this.members = response,
      error: error => console.log()
    })
  }
}