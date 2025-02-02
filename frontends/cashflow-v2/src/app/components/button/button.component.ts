import { twMerge} from 'tailwind-merge';
import { Component, Input } from '@angular/core';
import { LoaderComponent } from "../loader/loader.component";

@Component({
  selector: 'app-button',
  imports: [LoaderComponent],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  mergeClasses = twMerge

  @Input() className: string = '';
  @Input() disabled: boolean = false;
  @Input() loading: boolean = false;
}
