import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { City } from '../../models/city';
import { CityService } from '../../services/city.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { DisableControlDirective } from "../../directives/disable-control.directive";

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, DisableControlDirective],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;
  putCityForm: FormGroup;
  editCityId: string | null = null;

  private citiesService = inject(CityService);

  constructor() {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required])
    });

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    });
  }

  get putCityFormArray(): FormArray {
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities().subscribe(
      (response: City[]) => {
        this.cities = response;

        // Clear the form array before pushing new controls
        this.putCityFormArray.clear();

        this.cities.forEach(city => {
          this.putCityFormArray.push(new FormGroup({
            id: new FormControl(city.id, [Validators.required]),
            name: new FormControl({ value: city.name, disabled: true }, [Validators.required])
          }));
        });
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
      const newCity : any = {
        name: this.postCityForm.value.cityName
      };

      this.citiesService.postCity(newCity).subscribe({
        next: (response: City) => {
          console.log(response);
          this.cities.push(new City(response.id, response.name));
          this.putCityFormArray.push(new FormGroup({
            id: new FormControl(response.id, [Validators.required]),
            name: new FormControl({ value: response.name, disabled: true }, [Validators.required])
          }));
          this.postCityForm.reset();
        },
        error: (error: any) => {
          console.log(error);
        }
      });
    }
  }

  // Executes when the user clicks on 'Edit' button for the particular city
  editClicked(city: City): void {
    this.editCityId = city.id ?? null;
  }

  // Executes when the user clicks on 'Update' button after editing the city name
  updateClicked(i: number): void {
    this.citiesService.putCity(this.putCityFormArray.controls[i].value).subscribe({
      next: (response: string) => {
        console.log(response);

        this.editCityId = null;

        this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {}
    });
  }

  // Executes when the user clicks on 'Delete' button for the particular city
  deleteClicked(city: City,  i: number) : void {
    if (confirm(`Are you sure to delete to delete this city: ${city.id}?`)) {
      this.citiesService.deleteCity(city.id).subscribe({
        next: (response: string) => {
          console.log(response);

          this.putCityFormArray.removeAt(i);
          this.loadCities();
        }
      });
    }
  }
}
