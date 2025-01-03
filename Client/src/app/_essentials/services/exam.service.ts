import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exam } from '../models/exam';

@Injectable({
  providedIn: 'root'
})
export class ExamService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) {}

  getUserExams(username: string) {
    return this.http.get<Exam[]>(this.baseUrl + "examdll/" + username);
  }

  executeExamFile(file: File, username: string, copies: string, questions: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('username', username);
    formData.append('copies', copies);
    formData.append('questions', questions);
    return this.http.post(this.baseUrl + "examdll/execute_exam_file", formData, { responseType: 'blob' });
  }

  executeExam(username: string, examname: string, copies: string, questions: string) {
    const formData = new FormData();
    formData.append('username', username);
    formData.append('examname', examname);
    formData.append('copies', copies);
    formData.append('questions', questions);
    return this.http.post(this.baseUrl + "examdll/execute_exam", formData, { responseType: 'blob' });
  }

  deleteExamFile(username: string, examname: string) {
    const formData = new FormData();
    formData.append('username', username);
    formData.append('examname', examname);
    return this.http.post(this.baseUrl + "examdll/delete_exam_file", formData);
  }
}