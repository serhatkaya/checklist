import { Component } from '@angular/core';
import { AuthenticationService } from './services/auth.service';
@Component({
  selector: 'app-root',
  template: `
    <router-outlet></router-outlet>
  `,
  styles: [],
})
export class AppComponent {
  constructor(authService: AuthenticationService) {
    authService.currentUserSubject.next(JSON.parse(localStorage.getItem('currentUser')));
  }
}
