import { Routes } from '@angular/router';
import {CitiesComponent} from './components/cities/cities.component';
import {RegisterComponent} from "./components/register/register.component";
import {LoginComponent} from "./components/login/login.component";

export const routes: Routes = [
  { path:  "cities", component: CitiesComponent},
  { path:  "register", component: RegisterComponent},
  { path:  "login", component: LoginComponent}
];
