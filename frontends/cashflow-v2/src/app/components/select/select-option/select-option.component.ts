import { Component, EventEmitter, HostListener, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Option } from '../select.models';
import { twMerge } from 'tailwind-merge';
import { CdkMenuItem, CdkMenuModule } from '@angular/cdk/menu';

@Component({
  selector: 'app-select-option',
  imports: [],
  templateUrl: './select-option.component.html',
  styleUrl: './select-option.component.scss',
})
export class SelectOptionComponent {
  readonly mergeClasses = twMerge;

  readonly onSelectEvent = new EventEmitter<Option<any>>();

  @Input({ required: true }) option!: Option<any>
  @Input() className: string = '';

  constructor() { }

  select() {
    console.log({ select: this.option})
    this.onSelectEvent.emit(this.option)
  }
}
