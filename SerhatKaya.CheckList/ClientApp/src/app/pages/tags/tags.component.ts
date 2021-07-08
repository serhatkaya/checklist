import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html'
})
export class TagsComponent implements OnInit, OnDestroy {
  tagItem: any;
  private routeSub: Subscription;

  constructor(
    public router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute) {
    this.routeSub = this.route.params.subscribe(params => {
      this.httpClient.get('/api/tag/getCategories/' + params['id']).subscribe(res => {
        this.tagItem = res[0];
      });
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  ngOnInit(): void {
  }
}
