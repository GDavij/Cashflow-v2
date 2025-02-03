import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-input',
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    }
  ]
})
export class InputComponent implements ControlValueAccessor {
  mergeClasses = twMerge;

  @Input() placeholder: string = '';
  @Input() className: string = '';
  @Input() disabled: boolean = false;

  value: any;

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: (value: any) => any): void {
    this.onChange = fn;
  }
  
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  //#region <<Methods to Bind>>
  onChange(value: any): void { };

  onTouched(): void { }

  //#endregion

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  } 

}
