import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Exam } from 'src/app/_essentials/models/exam';
import { Member } from 'src/app/_essentials/models/member';
import { ExamService } from 'src/app/_essentials/services/exam.service';

@Component({
  selector: 'app-member-exam-item',
  templateUrl: './member-exam-item.component.html',
  styleUrls: ['./member-exam-item.component.css']
})
export class MemberExamItemComponent {
  @Input() exam: Exam | undefined;
  @Input() member: Member | undefined;
  genForm: FormGroup = new FormGroup({});
  process: boolean = false;
  generatedPdf: Blob | null = null;

  constructor(private examService: ExamService, private router: Router, private fb: FormBuilder) {}
  
  ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm() {
    this.genForm = this.fb.group({
      copies: ['', [Validators.required, Validators.pattern('^(?:[1-9][0-9]?|[1-4][0-9]{2}|500)$')]],
      questions: ['', [Validators.required, Validators.pattern('^(?:[1-9]|10)$')]]
    });
  }

  processEnded() {
    this.process = false;
  }

  generateExams() {
    this.process = true;

    var copies = this.genForm.controls['copies'].value;
    var questions = this.genForm.controls['questions'].value;

    this.examService.executeExam(this.member!.userName, this.exam!.examName, copies, questions).subscribe({
      next: (blob: Blob) => {
        this.generatedPdf = blob;
      },
      error: error => console.log('Error executing DLL:', error)
    });
  }

  onDownloadPdf(): void {
    if (this.generatedPdf) {
      const link = document.createElement('a');
      const url = window.URL.createObjectURL(this.generatedPdf);

      link.download = this.exam!.examName + '.pdf';
      link.href = url;
      link.click();

      window.URL.revokeObjectURL(url);
    } else {
      alert('No PDF available for download. Please generate it first.');
    }
  }

  deleteExam() {
    this.examService.deleteExamFile(this.member!.userName, this.exam!.examName).subscribe({
        next: response => {
          this.router.navigateByUrl('');
          console.log('Successful deletion!')
        },
        error: error => console.log(error)
      });
  }
}