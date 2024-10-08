import { Injectable } from '@angular/core';
import {City} from '../models/city';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CityService {
  cities : City[] = [];
  constructor(private httpClient : HttpClient) {

  }
  public getCities(): Observable<City[]> {
    return this.httpClient.get<City[]>("https://localhost:7100/api/v1/cities")
  }
}
