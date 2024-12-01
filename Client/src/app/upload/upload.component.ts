import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {
  @ViewChild('fileInput') fileInput!: ElementRef; // Reference to the file input element
  uploadedFile: File | null = null; // Store uploaded file
  isUploading = false; // Track upload state
  uploadProgress = 0; // Progress value for file upload
  isProcessing = false; // Track API processing state
  processingProgress = 0; // Progress value for API call
  apiErrorMessage = ''; // Error message from API
  downloadLink = ''; // URL for the downloadable file

  onFileDropped(event: DragEvent) {
    event.preventDefault();
    if (!event.dataTransfer || !event.dataTransfer.files.length) {
      return;
    }
    this.handleFile(event.dataTransfer.files[0]);
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.handleFile(file);
    }
  }

  preventDefault(event: Event) {
    event.preventDefault();
    event.stopPropagation();
  }

  handleFile(file: File) {
    this.uploadedFile = file; // Replace the previous file with the new one
    this.resetState();
    this.startUpload();
  }

  resetState() {
    this.isUploading = false;
    this.uploadProgress = 0;
    this.isProcessing = false;
    this.processingProgress = 0;
    this.apiErrorMessage = '';
    this.downloadLink = '';
  }

  startUpload() {
    this.isUploading = true;
    this.uploadProgress = 0;

    const uploadInterval = setInterval(() => {
      if (this.uploadProgress >= 100) {
        clearInterval(uploadInterval);
        this.isUploading = false;
      } else {
        this.uploadProgress += 10; // Simulate upload progress
      }
    }, 300);
  }

  generateExams() {
    if (!this.uploadedFile) {
      alert('Please upload a file first.');
      return;
    }

    this.isProcessing = true;
    this.apiErrorMessage = '';
    this.processingProgress = 0;

    const processingInterval = setInterval(() => {
      if (this.processingProgress >= 100) {
        clearInterval(processingInterval);

        // Simulate API success or error
        const isSuccess = Math.random() > 0.3; // 70% success rate
        if (isSuccess) {
          this.downloadLink = 'https://example.com/output.pdf'; // Replace with real API response link
        } else {
          this.apiErrorMessage = 'An error occurred while generating the exams. Please try again.';
        }
        this.isProcessing = false;
      } else {
        this.processingProgress += 10; // Simulate API call progress
      }
    }, 300);
  }

  downloadFile() {
    if (!this.downloadLink) {
      return;
    }
    const link = document.createElement('a');
    link.href = this.downloadLink;
    link.download = 'output.pdf';
    link.click();
  }
}