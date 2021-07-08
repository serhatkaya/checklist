import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Token } from '../models/user.i';
import { JwtHelper } from '../utilities/jwt.helper';
import { AuthenticationService } from './auth.service';
import { ToastService } from './toast.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authSvc: AuthenticationService,
    private toast: ToastService,
    private router: Router,
    private jwtHelper: JwtHelper,
    private authService: AuthenticationService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    const currentUser = this.authService.currentUserValue;
    let headers = new HttpHeaders();
    let decryptedToken: Token;
    if (currentUser) {
      decryptedToken = this.jwtHelper.decode(currentUser.token);
      headers = headers.append('Authorization', `Bearer ${currentUser.token}`);
    } else {
      this.router.navigate(['/login']);
    }

    if (decryptedToken != null && this.jwtHelper.isTokenExpired(decryptedToken.exp)) {
      this.toast.warning('Your session is expired, logging out!', 'Expired session.');
      decryptedToken = null;
      this.authSvc.logout();
    }

    request = request.clone({
      headers: headers,
    });

    return next.handle(request);
  }
}
