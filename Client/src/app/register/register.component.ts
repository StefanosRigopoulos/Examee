import { Component, OnInit, } from '@angular/core';
import { Router } from '@angular/router';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_essentials/services/account.service';
import { Lists } from '../_essentials/Lists';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();

  constructor(private accountService: AccountService, private router: Router, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 13);
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      userName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12), this.strongValidator()]],
      confirmPassword: ['', [Validators.required, this.matchValuesValidator('password')]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      role: ['', Validators.required],
      photoURL: [Lists.RandomPhotoURL[Math.floor(Math.random() * Lists.RandomPhotoURL.length)]] //Pick a random photo from our list.
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  matchValuesValidator(matchTo: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const password = control.parent?.get(matchTo)?.value;
      return password !== control.value ? { notMatching: true } : null;
    };
  }

  strongValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      let hasNumber = /\d/.test(control.value);
      let hasUpper = /[A-Z]/.test(control.value);
      let hasLower = /[a-z]/.test(control.value);
      const valid = hasNumber && hasUpper && hasLower;
      return !valid ? { alphanumeric: true } : null;
    };
  }

  get control() { return this.registerForm.controls }

  submit() {
    var checkbox = <HTMLInputElement> document.getElementById("checkForRegister");
    if (checkbox.checked == true) {
      let formValues = this.registerForm.getRawValue();
      const fullName = `${formValues.firstName}_${formValues.lastName}`;
      formValues.userName = fullName;

      this.accountService.register(formValues).subscribe({
        next: response => {
          console.log('Successful register!');
          this.router.navigateByUrl('/');
        },
        error: error => console.log(error)
      });
    } else {
      console.log('You have to agree to the terms of service!');
    }
  }
}
