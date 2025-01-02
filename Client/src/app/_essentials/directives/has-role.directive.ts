import { Directive, inject, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../services/account.service';

@Directive({
    selector: '[appHasRole]',
    standalone: true
})
export class HasRoleDirective implements OnInit{
  private accountService = inject(AccountService);
  
  @Input() appHasRole: string[] = [];
  user = this.accountService.currentUser();

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>) {}

  ngOnInit(): void {
    if (this.appHasRole.includes(this.user!.role)) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }
}