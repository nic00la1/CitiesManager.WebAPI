import { Injectable } from '@angular/core';
import {City} from '../models/city';


@Injectable({
  providedIn: 'root'
})
export class CityService {
  cities : City[] = [];
  constructor() {
    this.cities = [
        new("101", "New York"),
        new("102", "New Delhi"),
        new("103", "Sydney"),
        new("104", "Warsaw"),
      ];
  }

  public getCities(): City[] {
    return this.cities;
  }
}
