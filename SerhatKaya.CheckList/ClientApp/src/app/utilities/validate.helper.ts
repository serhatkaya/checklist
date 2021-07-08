import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ValidateHelper {
  validateEmail(email): boolean {
    const tester =
      /^[-!#$%&'*+\/0-9=?A-Z^_a-z`{|}~](\.?[-!#$%&'*+\/0-9=?A-Z^_a-z`{|}~])*@[a-zA-Z0-9](-*\.?[a-zA-Z0-9])*\.[a-zA-Z](-?[a-zA-Z0-9])+$/;
    if (!email) {
      return false;
    }
    if (email.length > 256) {
      return false;
    }
    if (!tester.test(email)) {
      return false;
    }
    const [account, address] = email.split('@');
    if (account.length > 64) {
      return false;
    }
    const [domain, ex] = address.split('.');
    return domain <= 63;
  }

  checkValid(data): boolean {
    return data === '' || data === null || data === undefined;
  }
}
