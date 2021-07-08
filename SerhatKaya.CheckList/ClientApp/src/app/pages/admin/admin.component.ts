import { themes } from './themes';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { Category } from 'src/app/models/category.i';
import { CheckList } from 'src/app/models/checklist.i';
import { Tags } from 'src/app/models/tags.i';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['admin.component.css']
})
export class AdminComponent implements OnInit, OnDestroy {
  @ViewChild('itemText', { static: false }) itemText: ElementRef;
  @ViewChild('itemHeader', { static: false }) itemHeader: ElementRef;

  themes = themes;

  tagItems = [];

  checkListDeleteMode = 0;
  tagDeleteMode = 0;
  categories: Category = {};
  tags: Tags;
  checkLists: any;
  assignedTagsModel: any;
  categoryModel: Category = {};
  categoryEditMode = false;
  categoryDeleteMode = 0;
  checklistModel: CheckList = {};
  checkListItems = [];
  checkListItemModel: any = [];
  userList: any;
  tagModel: any = {};
  tagEditMode = false;
  checklistEditMode = false;
  userEditMode = false;
  userModel: any = {};
  userDeleteMode = 0;
  logs: any = {};

  constructor(
    private toastr: ToastService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AdminComponent>,
    public router: Router,
    private httpClient: HttpClient) {
    this.getDatas('all');
  }

  newCategory() {
    this.httpClient.post('/api/category', this.categoryModel).subscribe(res => {
      this.getDatas('category');
      this.categoryModel.categoryName = '';
      this.categoryModel.description = '';
      this.categoryModel.tagItems = [];
    });
  }

  deleteCategory(cat) {
    this.httpClient.post('/api/category/remove', cat).subscribe(res => {
      this.getDatas('all');
    });
  }

  editCategory(cat) {
    this.categoryModel = {};
    this.categoryEditMode = true;
    const item = {
      id: cat.id,
      categoryName: cat.categoryName,
      description: cat.description,
      header: cat.header,
      theme: cat.theme,
      categoryId: cat.categoryId,
    };
    const tags = cat.tagItems;
    this.tagItems = tags;
    this.categoryModel = item;
  }

  sendEditCategory() {
    this.categoryModel.tagItems = [];
    for (const tagId of this.assignedTagsModel) {
      this.categoryModel.tagItems.push({ tagId: tagId, categoryId: this.categoryModel.id });
    }
    this.httpClient.post('/api/category/update', this.categoryModel).subscribe(res => {
      this.categoryEditMode = false;
      this.categoryModel = {};
      this.assignedTagsModel = [];
      this.tagItems = [];
      this.getDatas('category');
    });
  }

  clearCategoryModel() {
    this.categoryEditMode = false;
    this.categoryModel = {};
    this.assignedTagsModel = [];
    this.tagItems = [];
  }

  checkIfAssigned(id) {
    const pos = this.tagItems.map(function (e) {
      return e.tagId;
    }).indexOf(id);
    return pos !== -1;
  }

  newCheckList() {
    this.checklistModel.checkListItems = this.checkListItems;
    this.httpClient.post('/api/checklist', this.checklistModel).subscribe(res => {
      this.getDatas('checklist');
      this.toastr.success('Checklist successfully added!', 'Success');
      this.checklistModel = {};
      this.checkListItems = [];
      this.checkListItemModel = [];
    });
  }

  editCheckList(list) {
    this.checklistModel = {};
    const item = {
      id: list.id,
      description: list.description,
      header: list.header,
      checkListItems: list.checkListItems,
      categoryId: list.categoryId,
      theme: list.theme
    };
    this.checkListItems = [];
    for (const x of list.checkListItems) {
      this.checkListItems.push(x);
    }
    this.checklistModel = item;
    this.checklistEditMode = true;
  }

  sendEditCheckList() {
    this.checklistModel.checkListItems = this.checkListItems;
    this.httpClient.post('/api/checklist/update', this.checklistModel).subscribe(res => {
      this.getDatas('checklist');
      this.toastr.success('Checklist successfully updated!', 'Success');
      this.clearCheckListModel();
    });
  }

  deleteCheckList(list) {
    this.httpClient.post('/api/checklist/remove', list.id).subscribe(res => {
      this.getDatas('checklist');
      this.toastr.warning('Checklist removed successfully!', 'Deletion');
      this.checkListDeleteMode = 0;
    });
  }

  clearCheckListModel() {
    this.checklistModel = {};
    this.checkListItems = [];
    this.checkListItemModel = [];
    this.checklistEditMode = false;
  }

  checkListItemUp(item) {
    const pos = this.checkListItems.map(function (e) {
      return e.itemText;
    }).indexOf(item.itemText);
    console.log(pos);
    this.checkListItems = this.arrayMove(this.checkListItems, pos, pos - 1);
  }

  checkListItemDown(item) {
    const pos = this.checkListItems.map(function (e) {
      return e.itemText;
    }).indexOf(item.itemText);
    console.log(pos);
    this.checkListItems = this.arrayMove(this.checkListItems, pos, pos + 1);
  }

  newTag() {
    this.httpClient.post('/api/tag', this.tagModel).subscribe(res => {
      this.getDatas('tags');
      this.toastr.success('Tag successfully added!', 'Success');
      this.tagModel.tagName = '';
    });
  }

  editTag(tag) {
    this.tagModel = {};
    const item = {
      id: tag.id,
      tagName: tag.tagName
    };
    this.tagModel = item;
    this.tagEditMode = true;
  }

  sendEditTag(tag) {
    this.httpClient.post('/api/tag/edit', tag).subscribe(res => {
      this.getDatas('tags');
      this.toastr.success('Tag successfully updated!', 'Success');
      this.tagModel.tagName = '';
      this.tagEditMode = false;
    });
  }

  deleteTag(tag) {
    this.httpClient.post('/api/tag/remove', tag.id).subscribe(res => {
      this.getDatas('tags');
      this.toastr.warning('Tag removed successfully!', 'Deletion');
      this.tagDeleteMode = 0;
    });
  }


  removeChecklistItem(item) {

    const pos = this.checkListItems.map(function (e) {
      return e.itemHeader;
    }).indexOf(item.itemHeader);
    this.checkListItems.splice(pos, 1);
  }

  addChecklistItem() {
    const item = {
      itemHeader: this.checkListItemModel.itemHeader,
      itemText: this.checkListItemModel.itemText
    };
    this.checkListItems.push(item);
    this.checkListItemModel.itemHeader = '';
    this.checkListItemModel.itemText = '';
    this.itemHeader.nativeElement.focus();
    /*this.checkListItemModel.itemHeader = '';
    this.checkListItemModel.itemText = '';*/
  }

  addUser() {
    if (this.userModel.userFullName == undefined || this.userModel.role == undefined || this.userModel.email == undefined || this.userModel.password == undefined) {
      this.toastr.warning('Please fill the required fields!', 'Validation');
      return;
    }
    this.httpClient.post('/api/auth/register', this.userModel).subscribe(res => {
      this.getDatas('users');
      this.userModel = {};
      this.toastr.success('User successfully registered!', 'Success');
    });
  }

  editUser(user) {
    this.userEditMode = true;
    this.userModel.userFullName = user.userFullName;
    this.userModel.id = user.id;
    this.userModel.email = user.email;
    this.userModel.role = user.role;
  }

  sendEditUser() {
    this.httpClient.post('/api/auth/update', this.userModel).subscribe(res => {
      this.getDatas('users');
      this.toastr.success('User successfully updated!', 'Success');
      this.userEditMode = false;
      this.userModel = {};
    });
  }

  deleteUser(user) {
    this.httpClient.post('/api/auth/remove', user).subscribe(res => {
      this.getDatas('users');
      this.toastr.warning('User removed successfully!', 'Remove user');
      this.userDeleteMode = 0;
    });
  }

  getDatas(item) {
    if (item === 'category' || item === 'all') {
      this.httpClient.get('/api/category').subscribe(res => {
        this.categories = res;
      });
    }

    if (item === 'tags' || item === 'all') {

      this.httpClient.get('/api/tag').subscribe(res => {
        this.tags = res;
      });
    }
    if (item === 'checklist' || item === 'all') {
      this.httpClient.get('/api/checklist').subscribe(res => {
        this.checkLists = res;
      });
    }

    if (item === 'users' || item === 'all') {
      this.httpClient.get('/api/auth/getUsers').subscribe(res => {
        this.userList = res;
      });
    }
    if (item === 'logs' || item === 'all') {
      this.httpClient.get('/api/log').subscribe(res => {
        this.logs = res;
      });
    }
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  onChangeCategoryTagList(a) {
    this.categoryModel.tagItems = [];
    this.assignedTagsModel = a;
    for (const tag of this.assignedTagsModel) {
      this.categoryModel.tagItems.push({ tagId: tag });
    }
  }

  closeModal(): any {
    this.dialogRef.close(0);
  }

  arrayMoveMutate(array, from, to) {
    const startIndex = from < 0 ? array.length + from : from;

    if (startIndex >= 0 && startIndex < array.length) {
      const endIndex = to < 0 ? array.length + to : to;

      const [item] = array.splice(from, 1);
      array.splice(endIndex, 0, item);
    }
  }

  arrayMove(array, from, to) {
    array = [...array];
    this.arrayMoveMutate(array, from, to);
    return array;
  }

}
