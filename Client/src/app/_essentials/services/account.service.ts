import { HttpClient } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})

export class AccountService{
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);
  role = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const role = JSON.parse(atob(user.token.split('.')[1])).role;
      return role;
    }
    return null;
  })

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);  //Duplicate code.
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);  //Duplicate code.
        }
      })
    )
  }

  setCurrentUser(user: User) {
    //user.role = this.getDecodedToken(user.token).role;
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]))
  }
}
