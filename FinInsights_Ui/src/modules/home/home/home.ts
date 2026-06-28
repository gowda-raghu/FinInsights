import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { PathService } from '../../../common/services/path.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {

  username: string = '';
  constructor(private router: Router, private pathservice: PathService, private cd: ChangeDetectorRef) { }
  ngOnInit() {
    // example: get from localStorage or API
    let currentuser: any = localStorage.getItem('currentUser');
    currentuser = JSON.parse(currentuser);
    console.log(currentuser);
    this.username = currentuser["name"] || 'User';
    this.pathservice.updatePath(this.router.url);
    this.cd.detectChanges();
  }

  navigate(route: string) {
    this.router.navigate([`/${route}`]);
  }
}
