import { Component, OnInit } from "@angular/core";
import { MatDialog, MatDialogConfig } from "@angular/material";
import { AdminComponent } from "../pages/admin/admin.component";
import { AuthenticationService } from "../services/auth.service";

@Component({
  templateUrl: './master.component.html'
})
export class MasterComponent implements OnInit {

  constructor(
    public matDialog: MatDialog,
    public authSvc: AuthenticationService
  ) { }


  ngOnInit(): void {

  }

  openAdmin() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = false;
    dialogConfig.id = 'admin-modal';
    dialogConfig.height = '90%';
    dialogConfig.width = '95%';
    const modalDialog = this.matDialog.open(AdminComponent, dialogConfig);
    modalDialog.afterClosed().subscribe(res => {
    });
  }

  logOut = () => this.authSvc.logout();

  newTab(url: string) {
    window.open(url, "_blank");
  }
}
