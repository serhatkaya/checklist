import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user.i';
import { AuthenticationService } from '../../services/auth.service';
import { ToastService } from '../../services/toast.service';
import { ValidateHelper } from '../../utilities/validate.helper';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
  loginUser: User = {};

  constructor(
    public router: Router,
    private val: ValidateHelper,
    private toast: ToastService,
    private authSvc: AuthenticationService,
  ) {
    this.authSvc.currentUser.subscribe(res => {
      if (res != null) {
        router.navigate(['index']);
      }
    });
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  login(): any {
    if (this.val.checkValid(this.loginUser.email) || this.val.checkValid(this.loginUser.password)) {
      this.toast.warning('Please fill the required fields.', 'Validation');
    } else {
      this.authSvc.login(this.loginUser.email, this.loginUser.password)
      .toPromise();
    }
  }
}
