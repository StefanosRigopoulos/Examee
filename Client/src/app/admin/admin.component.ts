import { Component, OnInit } from '@angular/core';
import { MemberService } from '../_essentials/services/member.service';
import { Member } from '../_essentials/models/member';
import { AdminService } from '../_essentials/services/admin.service';
import { MatButtonModule } from '@angular/material/button';
import { NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css'],
    standalone: true,
    imports: [NgIf, NgFor, MatButtonModule]
})
export class AdminComponent implements OnInit {
  haveUsers: boolean = false;
  members: Member[] = [];

  constructor(private memberService: MemberService, private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers().subscribe({
      next: response => {
        this.members = response;
        this.haveUsers = true;
      }
    });
  }

  deleteUser(username: string) {
    this.adminService.deleteUser(username).subscribe({
      next: response => {
        console.log('Successful user deletion!')
        location.reload();
      },
      error: error => console.log(error)
    });
  }

  deleteUserExam(username: string, examname: string) {
    this.adminService.deleteUserExam(username, examname).subscribe({
      next: response => {
        console.log('Successful user\'s examname deletion!')
        location.reload();
      },
      error: error => console.log(error)
    });
  }
}
