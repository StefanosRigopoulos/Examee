import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from '../_essentials/services/account.service';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { TextInputComponent } from '../_essentials/forms/text-input/text-input.component';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule, TextInputComponent, MatButtonModule]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});

  constructor(public accountService: AccountService, private router: Router, private fb: FormBuilder) { }

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
      next: response => {
        console.log('Successful login!');
        this.router.navigateByUrl('/');
      },
      error: error => console.log()
    })
  }
}