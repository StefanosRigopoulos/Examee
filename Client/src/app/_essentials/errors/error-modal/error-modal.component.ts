import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-error-modal',
    templateUrl: './error-modal.component.html',
    styleUrls: ['./error-modal.component.css'],
    standalone: true,
    imports: [MatDialogModule, MatButtonModule]
})
export class ErrorModalComponent {
  constructor( public dialogRef: MatDialogRef<ErrorModalComponent>, @Inject(MAT_DIALOG_DATA) public data: any ) {}

  onClose(): void {
    this.dialogRef.close();
  }
}
