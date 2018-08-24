import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { User } from '../../Model/User';
import { SignIn, SignOut } from '../../Model/AuthStateModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  user = new User();
  loginError: boolean = false;
  warningMessage = "";
  constructor(private store: Store, private router: Router) { }

  ngOnInit() {
    this.signOut();
  }

  signOut(): void {
    this.store.dispatch(new SignOut()). subscribe(_ =>{});
  }
  onSubmit(): void {
    this.store.dispatch(new SignIn(this.user.profileName, this.user.password)).subscribe(
    _ => {
      this.router.navigate(['/feed']);
    },
     error=> {
      this.loginError = true;
    });
  }
}
