import { Component } from '@angular/core';
import { FileService } from '../_essentials/services/file.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  fileUrl: string | null = null;

  constructor(private fileService: FileService) {
    this.fileService.getFileDllURL().subscribe((url) => {
      this.fileUrl = url;
    });
  }
}
