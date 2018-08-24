import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { LoginRoutingModule } from './login-routing.module';
import { SignupComponent } from './signup/signup.component';
import { LoginComponent } from './login.component';


@NgModule({
  imports: [
    CommonModule,
    LoginRoutingModule,
    FormsModule,
    CommonModule
  ],
  declarations: [
    LoginComponent,
    SignupComponent
  ]
})
export class LoginModule { }
