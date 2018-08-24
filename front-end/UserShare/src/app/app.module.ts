import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin'; //state to survive page loads
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { PostComponent } from './Components/post/post.component';
import { AppRoutingModule } from './app-routing.module';

import { NavigationComponent } from './Components/navigation/navigation/navigation.component';
import { PostcommentsComponent } from './Components/post/postcomments/postcomments.component';
import { FeedComponent } from './Components/feed/feed.component';
import { FriendsComponent } from './Components/friends/friends.component';
import { SubscriptionsComponent } from './Components/subscriptions/subscriptions.component';
import { LikesComponent } from './Components/likes/likes.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { LoginModule } from './Components/login/login.module';
import { MakePostComponent } from './Components/post/make-post/make-post.component';
import { AddCommentComponent } from './Components/post/add-comment/add-comment.component';
import { SearchResultsComponent } from './Components/search-results/search-results.component';
import { AuthState } from './Model/AuthState';
import { AuthService } from './Services/auth.service';
import { CustomHttpInterceptor } from './http-interceptor';



@NgModule({
  declarations: [
    AppComponent,
    PostComponent,
    NavigationComponent,
    PostcommentsComponent,
    FeedComponent,
    FriendsComponent,
    SubscriptionsComponent,
    LikesComponent,
    ProfileComponent,
    MakePostComponent,
    AddCommentComponent,
    SearchResultsComponent,

  ],
  imports: [
    BrowserModule,
    NgxsModule.forRoot([AuthState]),
    NgxsStoragePluginModule.forRoot({
      key: ['auth.token','auth.profileName', 'auth.guid']
    }),
    HttpClientModule,
    FormsModule,
    LoginModule,
    AppRoutingModule
  ],
  providers: [
    AuthService, {
      provide: HTTP_INTERCEPTORS,
      useClass: CustomHttpInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
