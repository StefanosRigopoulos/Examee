import { Component, OnInit } from '@angular/core';
import { FileService } from '../_essentials/services/file.service';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: true,
    imports: [RouterLink, RouterLinkActive]
})
export class HomeComponent implements OnInit {
  exameeRendererURL: string | null = null;
  documentationURL: string | null = null;

  constructor(private fileService: FileService) { }

  ngOnInit(): void {
    this.fileService.getExameeRendererURL().subscribe((url) => {
      this.exameeRendererURL = url;
    });
    this.fileService.getDocumentationURL().subscribe((url) => {
      this.documentationURL = url;
    });
  }
}
