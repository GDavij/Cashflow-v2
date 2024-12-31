import { Component, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CacheService } from './services/cache.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'cashflow-v2';

}
