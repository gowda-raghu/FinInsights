import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class PathService {

    private path = new BehaviorSubject<string | null>(null);
    path$ = this.path.asObservable();
    updatePath(path: string) {
        this.path.next(path);
    }

}