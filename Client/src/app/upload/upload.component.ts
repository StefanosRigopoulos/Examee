import { Component, inject, OnInit } from '@angular/core';
import { FileService } from '../_essentials/services/file.service';
import { AccountService } from '../_essentials/services/account.service';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CanComponentDeactivate } from '../_essentials/guards/prevent-exit.guard';
import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ExamService } from '../_essentials/services/exam.service';
import { TextInputComponent } from '../_essentials/forms/text-input/text-input.component';
import { MatButtonModule } from '@angular/material/button';
import { HasRoleDirective } from '../_essentials/directives/has-role.directive';
import { NgIf, AsyncPipe } from '@angular/common';

@Component({
    selector: 'app-upload',
    templateUrl: './upload.component.html',
    styleUrls: ['./upload.component.css'],
    standalone: true,
    imports: [NgIf, HasRoleDirective, MatButtonModule, FormsModule, ReactiveFormsModule, TextInputComponent, RouterLink, RouterLinkActive]
})
export class UploadComponent implements OnInit, CanComponentDeactivate {
  private accountService = inject(AccountService);
  
  process: boolean = false;
  selectState: boolean = true;
  generateState: boolean = false;
  saveDownloadState: boolean = false;
  fileUploadedState: boolean = false;

  selectedFile: File | null = null;
  generatedPdf: Blob | null = null;

  genForm: FormGroup = new FormGroup({});

  progress: number = 0;
  user = this.accountService.currentUser();
  
  constructor(private fileService: FileService, private examService: ExamService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm() {
    this.genForm = this.fb.group({
      copies: ['', [Validators.required, Validators.pattern('^(?:[1-9][0-9]?|[1-4][0-9]{2}|500)$')]],
      questions: ['', [Validators.required, Validators.pattern('^(?:[1-9]|10)$')]]
    });
  }
  
  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.process) {
      return confirm('A file is being uploaded or processed. Do you really want to leave?');
    }
    return true;
  }

  processStarted() {
    this.process = true;
  }

  processEnded() {
    this.process = false;
  }

  uploadFile(): void {
    if (!this.selectedFile) return;
    this.fileService.uploadDll(this.selectedFile, this.user!.userName).subscribe({
      next: () => {
        this.fileUploadedState = true;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  generateExams() {
    this.selectState = false;       // File selected ended.
    this.generateState = true;      // Generate process started.

    if (!this.selectedFile) return;

    var copies = this.genForm.controls['copies'].value;
    var questions = this.genForm.controls['questions'].value;

    this.examService.executeExamFile(this.selectedFile, this.user!.userName, copies, questions).subscribe({
      next: (blob: Blob) => {
        this.generatedPdf = blob;
        this.generateState = false;     // Generate process ended.
        this.saveDownloadState = true;  // Save or Download process started.
      },
      error: error => {
        this.generateState = false;     // Generate stopped.
        this.selectState = true;        // Return to select state.
        this.selectedFile = null;       // Empty the selected file.
        this.processEnded();            // Start the process from the start.
      }
    });
  }

  // Downloads the stored PDF Blob
  onDownloadPdf(): void {
    if (this.generatedPdf) {
      const link = document.createElement('a');
      const url = window.URL.createObjectURL(this.generatedPdf);
      const nameParts = this.selectedFile!.name.split('.');

      nameParts.pop();
      link.download = nameParts + '.pdf';
      link.href = url;
      link.click();

      window.URL.revokeObjectURL(url);
    } else {
      alert('No PDF available for download. Please generate it first.');
    }
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