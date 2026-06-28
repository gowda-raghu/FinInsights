import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from '../modules/login/login';
import { Alert } from '../common/alert/alert';
import { NavChartComponent } from '../common/shared/nav-chart/nav-chart';
import { LoaderService } from '../services/loaderService';
import { LoaderComponent } from '../common/shared/loader/loader';
import { TopPanel } from '../common/top-panel/top-panel';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,Login,Alert,NavChartComponent,LoaderComponent,TopPanel],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('test');
}
