import {User} from './User';
import {Post} from './Post';
import { Friend } from './Friend';

export class UserClient {
    user: User;
    posts: Post[];
    friends: Friend[];
}
