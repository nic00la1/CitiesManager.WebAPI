import {Component, inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {City} from "../models/city";
import {CityService} from "../services/city.service";

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];

  private citiesService = inject(CityService);

  ngOnInit(){
    this.citiesService.getCities()
      .subscribe(
        (response: City[]) => {
          this.cities = response;
        },
        (error: any) => {
          console.log(error);
        },
        () => {}
      );
  }
}
