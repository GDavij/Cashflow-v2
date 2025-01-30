import { twMerge} from 'tailwind-merge';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  mergeClasses = twMerge

  @Input() className: string = '';
  @Input() disabled: boolean = false;
  @Input() loading: boolean = false;
}
