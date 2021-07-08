import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../models/category.i';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html'
})
export class CategoriesComponent implements OnInit, OnDestroy {
  categoryItems: Category;
  constructor(
    public router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute) {
    this.httpClient.get('/api/category').subscribe(res => {
      this.categoryItems = res;
    });
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }
}
