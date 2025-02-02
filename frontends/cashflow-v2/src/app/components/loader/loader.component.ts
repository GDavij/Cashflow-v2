import { Component, Input } from '@angular/core';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-loader',
  imports: [],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss'
})
export class LoaderComponent {
  mergeClasses = twMerge;

  @Input({ required: true }) isLoading!: boolean;

  @Input() className: string = '';
  @Input() innerLoaderClassName: string = '';
}
