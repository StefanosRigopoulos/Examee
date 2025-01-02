import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { map, of } from 'rxjs';
import { Member } from '../models/member';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  baseUrl = environment.apiUrl;
  members: Member[] = [];
  user = this.accountService.currentUser();

  constructor() {}

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'user/' + username)
  }

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'user').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }
}
