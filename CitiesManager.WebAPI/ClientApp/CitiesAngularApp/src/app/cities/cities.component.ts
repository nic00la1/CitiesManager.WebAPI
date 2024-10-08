import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { City } from '../models/city';
import { CityService } from '../services/city.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  private citiesService = inject(CityService);

  constructor() {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required])
    });
  }

  loadCities() {
    this.citiesService.getCities().subscribe(
      (response: City[]) => {
        this.cities = response;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  ngOnInit() {
    this.loadCities();
  }

  get postCity_CityNameControl(): any {
    return this.postCityForm.controls['cityName'];
  }

  postCitySubmitted() {
    this.isPostCityFormSubmitted = true;

    if (this.postCityForm.valid) {
      const newCity = {
        name: this.postCityForm.value.cityName
      };

      this.citiesService.postCity(newCity).subscribe({
        next: (response: City) => {
          console.log(response);
          this.cities.push(new City(response.id, response.name));
          this.postCityForm.reset();
        },
        error: (error: any) => {
          console.log(error);
        }
      });
    }
  }
}
