import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../Services/user.service';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/operators';
import { Store } from '@ngxs/store';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  isInitialPage: boolean = true;
  currentPage: string;
  constructor(private store: Store,private userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  initSearch(query: string): void {
    if (this.isInitialPage)
    {
      this.currentPage = this.router.url;
      this.isInitialPage = false;
    }
    if (query != "")
    {
      this.userService.setSearchQuery(query);
      this.router.navigate(['/search-results']);
    }
    else 
    {
      const token = this.store.selectSnapshot(state => state.auth.token);
      if(this.currentPage == '/login' && token != null)
        this.currentPage = '/feed';
      this.router.navigate([this.currentPage]);
    }      
  }
  setCurrentPage(url: string): void {
    this.currentPage = url;
    document.getElementById("searchUsers").nodeValue = "";
  }
}
