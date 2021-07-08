import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class JwtHelper {
  // ### Swifty Jwt Helper
  decode(token) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  }

  isTokenExpired(exp: string, offsetSeconds?: number): boolean {
    if (!exp || exp === '') {
      return true;
    }
    const date = new Date(0);
    date.setUTCSeconds(Number(exp));
    offsetSeconds = offsetSeconds || 0;
    return date === null ? false : !(date.valueOf() > new Date().valueOf() + offsetSeconds * 1000);
  }
}
