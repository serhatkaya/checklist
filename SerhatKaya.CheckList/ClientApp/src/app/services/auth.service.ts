import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Token, User } from '../models/user.i';
import { JwtHelper } from '../utilities/jwt.helper';
import { ToastService } from './toast.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  path = '/api/auth/';
  user: Token = null;

  constructor(private http: HttpClient,
    private router: Router,
    private toastr: ToastService,
    public jwtHelper: JwtHelper) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(email: string, password: string) {
    const loginModel = {
      email: email,
      password: password
    };
    return this.http.post<any>(this.path, loginModel)
      .pipe(map(user => {
        if (user) {
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.user = this.jwtHelper.decode(user.token);
          this.currentUserSubject.next(user);
          this.toastr.success('Successfully logged in! <br> Redirecting..', 'Welcome ' + user.userFullName);
          this.router.navigate(['/index']);
        }
        return user;
      }));
  }

  logout() {
    this.user = null;
    this.toastr.info('Logging out...', 'Log out');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }
}
