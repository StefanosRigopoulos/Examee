import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, take } from 'rxjs';
import { Exam } from 'src/app/_essentials/models/exam';
import { Member } from 'src/app/_essentials/models/member';
import { User } from 'src/app/_essentials/models/user';
import { AccountService } from 'src/app/_essentials/services/account.service';
import { MemberService } from 'src/app/_essentials/services/member.service';

@Component({
  selector: 'app-member-profile',
  templateUrl: './member-profile.component.html',
  styleUrls: ['./member-profile.component.css']
})
export class MemberProfileComponent implements OnInit {
  member: Member = {} as Member;
  exams: Exam[] = [] as Exam[];
  user?: User;

  constructor(private accountService: AccountService, private memberService: MemberService, private router: ActivatedRoute) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    })
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if(!this.user) return;
    this.memberService.getMember(this.user.userName).subscribe({
      next: member => {
        this.member = member
        this.exams = this.member.exams
      }
    })
  }
}
