import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckList } from '../../models/checklist.i';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html'
})
export class IndexComponent implements OnInit, OnDestroy {
  constructor(
    public router: Router,
    private httpClient: HttpClient
  ) {
    this.getIndexData();
  }

  cLItem: CheckList;
  tags: any;


  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  getIndexData() {
    this.httpClient.get('/api/checklist').subscribe(res => {
      this.cLItem = res;
    });
    this.httpClient.get('/api/tag').subscribe(res => {
      this.tags = res;
    });
  }
}
