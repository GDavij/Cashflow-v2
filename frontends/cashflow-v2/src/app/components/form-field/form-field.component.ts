import { Component, Input } from '@angular/core';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-form-field',
  imports: [],
  templateUrl: './form-field.component.html',
  styleUrl: './form-field.component.scss'
})
export class FormFieldComponent {
  readonly mergeClasses = twMerge;

  @Input() className: string = '';

  //TODO: REFACTOR TO USE "REFLECTION" via query list of <app-input> components
  @Input() required: boolean = false;
  @Input({ required: true }) label: string = '';
}
