import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';
import { ChecklistDetailComponent } from './checklist-detail/checklist-detail.component';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { TagsComponent } from './tags/tags.component';

@NgModule({
  declarations: [IndexComponent, ChecklistDetailComponent, CategoriesComponent, LoginComponent, CategoryDetailComponent, TagsComponent],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: 'index', pathMatch: 'full',
        component: IndexComponent
      },
      {
        path: 'checklist/:id',
        component: ChecklistDetailComponent
      },
      {
        path: 'categories',
        component: CategoriesComponent
      },
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'category/:id',
        component: CategoryDetailComponent
      },
      {
        path: 'tags/:id',
        component: TagsComponent
      }
    ])
  ]
})
export class PagesModule {

}
