import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-category-details',
  templateUrl: './category-detail.component.html'
})
export class CategoryDetailComponent implements OnInit, OnDestroy {
  private routeSub: Subscription;
  categoryItem: any;

  constructor(
    public router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute) {
    this.routeSub = this.route.params.subscribe(params => {
      this.httpClient.get('/api/category/detail/' + params['id']).subscribe(res => {
        this.categoryItem = res;
      });
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  ngOnInit(): void {
  }
}
