import { Component, OnInit } from '@angular/core';
import { UserService } from '../../Services/user.service';
import { User } from '../../Model/User';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  users: User[];
  public users$: Observable<User[]>;
  constructor(private userService: UserService) { }
  hasLoaded: boolean = false;

  ngOnInit() {
    this.getSearchResults();
  }

  getSearchResults(): void {
      this.users$ = this.userService.searchQuery.pipe(
        distinctUntilChanged(),
        debounceTime(300),
        switchMap(() => 
          this.userService.getSearchResults()),
        );
  }

}
