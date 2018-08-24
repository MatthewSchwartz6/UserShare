import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PostComponent} from './Components/post/post.component';
import { FeedComponent } from './Components/feed/feed.component';
import { LikesComponent } from './Components/likes/likes.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { SubscriptionsComponent } from './Components/subscriptions/subscriptions.component';
import { FriendsComponent } from './Components/friends/friends.component';
import { AuthGuard } from './auth.guard';
import { SearchResultsComponent } from './Components/search-results/search-results.component';

const routes: Routes = [
{ path: 'feed', component: FeedComponent,canActivate:[AuthGuard]},
{ path: 'likes', component: LikesComponent,canActivate:[AuthGuard]},
{ path: 'profile/:id', component: ProfileComponent,canActivate:[AuthGuard]},
{ path: 'subscriptions', component: SubscriptionsComponent, canActivate:[AuthGuard]},
{ path: 'friends', component: FriendsComponent, canActivate:[AuthGuard]},
{path: 'login', loadChildren: 'app/Components/login/login.module#LoginModule'},
{path: 'search-results', component: SearchResultsComponent, canActivate:[AuthGuard]},
{ path: '', redirectTo : '/login', pathMatch: 'full'}
];
// profile should be the component that either displays your profile if you click on the brand anchor in navigation
// or a friends profile if you click on their name from a post on your feed/ search for it
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
