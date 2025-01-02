import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css'],
    standalone: true,
    imports: [MatButtonModule, RouterLink, RouterLinkActive]
})
export class NotFoundComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {}
}