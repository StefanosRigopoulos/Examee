import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) {}

  getExameeRendererURL() {
    return this.http.get(this.baseUrl + "file/get-examee-renderer", { responseType: 'text' });
  }

  getDocumentationURL() {
    return this.http.get(this.baseUrl + "file/get-documentation", { responseType: 'text' });
  }

  uploadDll(file: File, username: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('username', username);
    return this.http.post(this.baseUrl + "file/upload-dll", formData);
  }
}
