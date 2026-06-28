import { Component } from '@angular/core';
import { AlertService } from './alert.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alert',
  imports: [CommonModule],
  templateUrl: './alert.html',
  styleUrl: './alert.scss',
})
export class Alert {

  message: string | null = null;

  constructor(private alertService: AlertService) { }

  ngOnInit() {
    this.alertService.message$.subscribe(msg => {
      this.message = msg;
    });
  }

  close() {
    this.alertService.clear();
  }
}
