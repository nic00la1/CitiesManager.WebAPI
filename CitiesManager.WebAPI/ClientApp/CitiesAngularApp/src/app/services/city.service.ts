import { Injectable } from '@angular/core';
import {City} from '../models/city';

@Injectable({
  providedIn: 'root'
})
export class CityService {
  cities : City[] = [];
  constructor() {
    this.cities = [
        new City("101", "New York"),
        new City("102", "New Delhi"),
        new City("103", "Sydney"),
        new City("104", "Warsaw"),
      ];
  }
  public getCities(): City[] {
    return this.cities;
  }
}
