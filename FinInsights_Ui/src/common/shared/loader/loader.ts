import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // ✅ IMPORTANT
import { LoaderService } from '../../../services/loaderService';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule], // ✅ ADD THIS
  templateUrl: './loader.html',
  styleUrl: './loader.scss',
})
export class LoaderComponent {

  loading$: any;
  text$: any;

  constructor(private _loaderService: LoaderService) {
    this.loading$ = this._loaderService.loading$;
    this.text$ = this._loaderService.text$;
  }
}



