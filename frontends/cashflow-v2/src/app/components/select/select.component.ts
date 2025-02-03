import { AfterViewInit, ChangeDetectorRef, Component, ContentChildren, forwardRef, Input, OnChanges, OnInit, QueryList, SimpleChanges, ViewChild, ViewRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, SelectControlValueAccessor } from '@angular/forms';
import { twMerge } from 'tailwind-merge';
import { Option } from './select.models';
import { CdkMenuTrigger } from '@angular/cdk/menu';
import { SelectContainerComponent } from './select-container/select-container.component';

@Component({
  selector: 'app-select',
  imports: [CdkMenuTrigger],
  templateUrl: './select.component.html',
  styleUrl: './select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true
    }
  ]
})
export class SelectComponent implements ControlValueAccessor, AfterViewInit, OnChanges {
  @ContentChildren(SelectContainerComponent) selectContainer!: QueryList<SelectContainerComponent>;
  @ViewChild(CdkMenuTrigger) menu!: CdkMenuTrigger;  

  readonly mergeClasse = twMerge

  @Input({ required: true }) options: Option<any>[] = [];
  @Input() notFoundText: string = '';
  @Input() className: string = '';
  @Input() disabled: boolean = false;
  @Input() isStaticOptions: boolean = false;

  value: any;
  isOpen: boolean = false;
  alreadyHasBeenOpened: boolean = false;

  constructor() { }

  ngAfterViewInit(): void {

    this.selectContainer.forEach(container => {
      container.onSelectionEvent.subscribe(option => {
        this.writeValue(option.value);
        this.onChange(option.value);
        this.menu.close();
      })
      
      container.onBlurEvent.subscribe(() => {
        this.menu.close();
      })
  })
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.selectContainer?.first) {
      this.selectContainer.first.onOptionsChangeEvent.emit();
    }
  }

  onOpenContainer() {
    if (this.isStaticOptions && !this.alreadyHasBeenOpened) {
      this.selectContainer.first.onOptionsChangeEvent.emit(this.isStaticOptions);
      this.alreadyHasBeenOpened = true;
    }
  }


  get optionName() {
    return this.options.find(opt => opt.value == this.value)?.label || this.notFoundText || "Select one option";
  }

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
