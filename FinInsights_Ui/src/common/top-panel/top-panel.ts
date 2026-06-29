import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PathService } from '../services/path.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-top-panel',
  imports: [FormsModule, CommonModule],
  templateUrl: './top-panel.html',
  styleUrl: './top-panel.scss',
})
export class TopPanel {

  userName: string = '';
  showMenu = false;
  searchText = '';

  isDark = true;
  isAvatar = true;
  constructor(private router: Router, private pathService: PathService, private cd: ChangeDetectorRef) {

    let currentuser: any = localStorage.getItem('currentUser');
    if (currentuser) {
      currentuser = JSON.parse(currentuser);
      console.log(currentuser);
      this.userName = currentuser["name"] || 'User';
      this.loadTheme();
    }
  }

  ngOnInit() {
    this.pathService.path$.subscribe((path: any) => {
      if (path === '/' || this.router.url == '/') {
        this.isAvatar = false; //this.cd.detectChanges() 
      }
      else {
        this.isAvatar = true;
        this.cd.detectChanges();
      }
      console.log("hfbfbchh")
      console.log(this.isAvatar);
      console.log(this.router.url);
    })
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  onSearch() {
    console.log(this.searchText);
  }

  onHelp() {

    this.router.navigate(['/help']);

  }

  onAdmin() {
    this.router.navigate(['/admin']);
  }

  goProfile() {
    this.router.navigate(['/profile']);
  }

  goSettings() {
    this.router.navigate(['/settings']);
  }


  logout(): void {

    localStorage.clear();
    sessionStorage.clear();

    this.router.navigateByUrl('/').then(() => {
      window.location.reload();
    });

  }


  /* 🔥 AVATAR INITIALS */
  getInitials(name: string): string {
    console.log(name, name
      .split(' ')
      .map(x => x[0])
      .join('')
      .toUpperCase());
    return name
      .split(' ')
      .map(x => x[0])
      .join('')
      .toUpperCase();
  }

  /* 🌙 THEME TOGGLE */
  loadTheme(): void {

    const savedTheme = localStorage.getItem('theme');

    this.isDark = savedTheme !== 'light';

    document.body.classList.toggle('light-theme', !this.isDark);
    document.body.classList.toggle('dark-theme', this.isDark);


  }

  toggleTheme(): void {

    this.isDark = !this.isDark;

    document.body.classList.toggle('light-theme', !this.isDark);
    document.body.classList.toggle('dark-theme', this.isDark);


    localStorage.setItem(
      'theme',
      this.isDark ? 'dark' : 'light'
    );

  }
}