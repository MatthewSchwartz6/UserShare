import { Component, OnInit } from '@angular/core';
import { User } from '../../../Model/User';
import { LoginService } from '../login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  public user = new User();
  public reenteredPassword = "";
  passwordsNotMatched: boolean = false;
  userAlreadyExists: boolean = false;
  constructor(private loginService: LoginService, private router: Router) { }

  ngOnInit() {
  }
  onSubmit(): void {
    if (!this.checkReenteredPassword())
    {
      this.passwordsNotMatched = true;
      return;
    }
    this.loginService.SignUp(this.user).subscribe((s: any)=> {
      if (s.response == "User created successfully.")
      {
//        console.log(s.response);
        this.router.navigate(['/login']);
      }
      else if(s.response == "User already exists.")
      {
        this.userAlreadyExists = true;
        return;
      }
    }, error => {
      
      console.log("twas an error.");
    });
  }
  checkReenteredPassword(): Boolean {

    return this.user.password == this.reenteredPassword;

  }

}
