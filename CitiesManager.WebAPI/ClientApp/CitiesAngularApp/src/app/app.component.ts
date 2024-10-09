import {Component, inject} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {AccountService} from "./services/account.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  public accountService = inject(AccountService);
  public router = inject(Router);

  onLogoutClicked() {
    this.accountService.getLogout().subscribe(({
      next: (response: string) => {
        this.accountService.currentUserName = null;

        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.log(error);
      }
    }));
  }
}
