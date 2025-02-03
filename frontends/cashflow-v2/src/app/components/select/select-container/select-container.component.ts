import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ContentChildren,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  OnInit,
  QueryList,
  SimpleChanges,
  ViewChild,
  ViewRef,
} from '@angular/core';
import { Option } from '../select.models';
import { twMerge } from 'tailwind-merge';
import { SelectOptionComponent } from '../select-option/select-option.component';
import { CdkMenu } from '@angular/cdk/menu';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-select-container',
  imports: [CdkMenu],
  templateUrl: './select-container.component.html',
  styleUrl: './select-container.component.scss',
})
export class SelectContainerComponent implements AfterViewInit {
  @ViewChild('container') element!: ElementRef;
  @ContentChildren(SelectOptionComponent, { descendants: true }) options!: QueryList<SelectOptionComponent>;

  readonly mergeClasses = twMerge;

  readonly onSelectionEvent = new EventEmitter<Option<any>>();

  readonly onBlurEvent = new EventEmitter<void>();
  readonly onOptionsChangeEvent = new EventEmitter<boolean>();

  readonly optionsSubscriptions: Subscription[] = [];

  @Input() className: string = '';
  @Input() notFoundText: string = 'No Options to Select';

  constructor(private readonly _changeDetectorRef: ChangeDetectorRef) {}

  ngAfterViewInit(): void {
    console.log({options: {...this.options}})
    this.options.changes.subscribe(() => this.configureOptions());
    this.onOptionsChangeEvent.subscribe((isStaticOptions: boolean) => this.LookupForOptions(isStaticOptions))
  }

  configureOptions() {
    console.log("CONFIGURING OPTIONS")
    this._changeDetectorRef.detectChanges();
    while (this.optionsSubscriptions?.length) {
      this.optionsSubscriptions.pop()!.unsubscribe();
    }

    this.options.forEach(opt => {
      this.optionsSubscriptions.push(opt.onSelectEvent.subscribe(opt => {
        this.onSelectionEvent.emit(opt);
      }))
    })
  }

  // IMPROVE: HANDLE STATIC OPTIONS DETECTION AND EVENT BINDING. 
  LookupForOptions(isStaticOptions: boolean) {
    if (isStaticOptions) {
      this.configureOptions();
    }

    this._changeDetectorRef.detectChanges();
  }

  get isEmpty() {
    return this.options.length === 0;
  }

  onBlur() {
    this.onBlurEvent.emit();
  }

  //TODO: Implement a focus call to set elegible for blur event
  focus() {}
}
