import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {AccountService} from "../../services/account.service";
import {Router} from "@angular/router";
import {CompareValidation} from "../../validators/custom-validators";
import {RegisterUser} from "../../models/register-user";
import {LoginUser} from "../../models/login-user";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoginFormSubmitted: boolean = false;

  private accountService = inject(AccountService);
  private router = inject(Router);

  constructor() {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required]),
    });
  }

  get loginEmailControl(): any {
    return this.loginForm.get('email');
  }

  get loginPasswordControl(): any {
    return this.loginForm.get('password');
  }

  loginSubmitted() {
    this.isLoginFormSubmitted = true;

    if (this.loginForm.valid) {

      this.accountService.postLogin(this.loginForm.value).subscribe({
        next: (response: any) => {
          console.log(response);

          this.isLoginFormSubmitted = false;
          this.accountService.currentUserName = response.email;
          localStorage["token"] = response.token;

          this.router.navigate(['/cities']);

          this.loginForm.reset();
        },
        error: (error) => {
          console.error(error);
        },
        complete: () => {
        }
      })
    }
  }
}
