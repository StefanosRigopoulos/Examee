import { Component, ElementRef, ViewChild } from '@angular/core';
import { FileService } from '../_essentials/services/file.service';
import { AccountService } from '../_essentials/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {
  selectedFile: File | null = null;
  uploadedFile: File | null = null;
  progress: number = 0;
  downloadUrl: string | null = null;

  constructor(public accountService: AccountService, private fileService: FileService, private router: Router) {}

  uploadFile(): void {
    if (!this.selectedFile) return;
    this.fileService.uploadDll(this.selectedFile).subscribe({
      next: (event) => {
        if (event.status === 'progress') {
          this.progress = event.progress;
        } else if (event.status === 'complete') {
          this.downloadUrl = event.body?.url || null;
          this.uploadedFile = this.selectedFile;
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  generateExams() {
    
  }

  onFileSelected(file: File | null): void {
    if (!file) return;
    if (!this.isDllFile(file)) return;
    this.selectedFile = file;
  }

  preventDefault(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  private isDllFile(file: File): boolean {
    const extension = file.name.split('.').pop()?.toLowerCase();
    return extension === 'dll';
  }
}