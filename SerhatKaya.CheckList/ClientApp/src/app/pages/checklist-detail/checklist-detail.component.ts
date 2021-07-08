import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CheckList } from '../../models/checklist.i';
import { AuthenticationService } from '../../services/auth.service';
import { ToastService } from './../../services/toast.service';

@Component({
  selector: 'app-checklist-details',
  templateUrl: './checklist-detail.component.html'
})
export class ChecklistDetailComponent implements OnInit, OnDestroy {
  cLItem: CheckList;
  checkedItems = [];

  constructor(
    public router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private authSvc: AuthenticationService,
    private toastr: ToastService) {
    this.route.params.subscribe(params => {
      this.httpClient.get('/api/checklist/detail/' + params['id']).subscribe(res => {
        this.cLItem = res;
      });
    });
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  checkIfChecked(id) {
    if (this.checkedItems.includes(id)) {
      return 'true';
    }
    return 'false';
  }

  checkItem(id) {
    if (this.checkedItems.includes(id)) {
      const index = this.checkedItems.indexOf(id);
      this.checkedItems.splice(index, 1);
    } else {
      this.checkedItems.push(id);
    }
  }

  sendLog(id) {
    if (this.checkedItems.length == this.cLItem.checkListItems.length) {
      const logItem = {
        userId: this.authSvc.currentUserValue.id,
        checklistId: id
      };
      this.httpClient.post('/api/log/confirm', logItem).subscribe(res => {
        this.toastr.success('Confirmation success!', 'Success');
        this.router.navigate(['/index']);
      });
    }
  }
}
