import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();

  private textSubject = new BehaviorSubject<string>('Loading...');
  text$ = this.textSubject.asObservable();

  show(text: string = 'Loading...') {
    this.textSubject.next(text);
    this.loadingSubject.next(true);
  }

  hide() {
    this.loadingSubject.next(false);
  }
}