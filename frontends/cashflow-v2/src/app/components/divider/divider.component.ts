import { Component, Input } from '@angular/core';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-divider',
  imports: [],
  templateUrl: './divider.component.html',
  styleUrl: './divider.component.scss'
})
export class DividerComponent {
  mergeClasses = twMerge;

  @Input() className: string = '';
}
