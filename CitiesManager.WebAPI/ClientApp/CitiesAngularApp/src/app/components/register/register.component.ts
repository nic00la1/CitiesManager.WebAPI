import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {AccountService} from "../../services/account.service";
import {Router} from "@angular/router";
import {RegisterUser} from "../../models/register-user";
import {CompareValidation} from "../../validators/custom-validators";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;
  isRegisterFormSubmitted: boolean = false;

  private accountService = inject(AccountService);
  private router = inject(Router);

  constructor() {
    this.registerForm = new FormGroup({
      personName: new FormControl(null,  [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      phoneNumber: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      confirmPassword: new FormControl(null, [Validators.required]),
    }, {
      validators: [CompareValidation('password', 'confirmPassword')]
    });
  }

  get registerPersonNameControl(): any {
    return this.registerForm.get('personName');
  }

  get registerEmailControl(): any {
    return this.registerForm.get('email');
  }

  get registerPhoneNumberControl(): any {
    return this.registerForm.get('phoneNumber');
  }

  get registerPasswordControl(): any {
    return this.registerForm.get('password');
  }

  get registerConfirmPasswordControl(): any {
    return this.registerForm.get('confirmPassword');
  }

  registerSubmitted() {
    this.isRegisterFormSubmitted = true;

    if (this.registerForm.valid) {

      this.accountService.postRegister(this.registerForm.value).subscribe({
        next: (response: any) => {
          console.log(response);

          this.isRegisterFormSubmitted = false;

          this.router.navigate(['/cities']);
          localStorage["token"] = response.token;

          this.registerForm.reset();
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
