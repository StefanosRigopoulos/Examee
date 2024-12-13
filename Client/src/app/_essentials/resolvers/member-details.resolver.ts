import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { MemberService } from '../services/member.service';
import { Member } from '../models/member';

export const MemberDetailResolver: ResolveFn<Member> = (route) => {
  const memberService = inject(MemberService);
  const username = route.paramMap.get('username')!;
  return memberService.getMember(username);
};