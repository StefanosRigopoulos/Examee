import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Exam } from 'src/app/_essentials/models/exam';
import { Member } from 'src/app/_essentials/models/member';
import { AccountService } from 'src/app/_essentials/services/account.service';
import { MemberService } from 'src/app/_essentials/services/member.service';
import { MemberExamItemComponent } from '../member-exam-item/member-exam-item.component';
import { NgFor } from '@angular/common';

@Component({
    selector: 'app-member-profile',
    templateUrl: './member-profile.component.html',
    styleUrls: ['./member-profile.component.css'],
    standalone: true,
    imports: [NgFor, MemberExamItemComponent, RouterLink]
})
export class MemberProfileComponent implements OnInit {
  private accountService = inject(AccountService);
  
  member: Member = {} as Member;
  exams: Exam[] = [] as Exam[];
  user = this.accountService.currentUser();

  constructor(private memberService: MemberService) {}

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
