import { Component } from '@angular/core';
import { FileService } from '../_essentials/services/file.service';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: true,
    imports: [RouterLink, RouterLinkActive]
})
export class HomeComponent {
  dllUrl: string | null = null;
  documentationUrl: string | null = null;

  constructor(private fileService: FileService) {
    this.fileService.getFileDllURL().subscribe((url) => {
      this.dllUrl = url;
    });
    this.fileService.getDocumentationPDFURL().subscribe((url) => {
      this.documentationUrl = url;
    });
  }
}
