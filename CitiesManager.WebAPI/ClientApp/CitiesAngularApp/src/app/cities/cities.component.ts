import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {

}
