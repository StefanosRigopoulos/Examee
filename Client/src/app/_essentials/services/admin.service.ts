import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  deleteUser(username: string) {
    const formData = new FormData();
    formData.append('username', username);
    return this.http.post(this.baseUrl + "admin/delete_user", formData);
  }

  deleteUserExam(username: string, examname: string) {
    const formData = new FormData();
    formData.append('username', username);
    formData.append('examname', examname);
    return this.http.post(this.baseUrl + "admin/delete_user_exam", formData);
  }
}