import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) {}

  getFileDllURL() {
    return this.http.get(this.baseUrl + "file/get-dll", { responseType: 'text' });
  }

  getDocumentationPDFURL() {
    return this.http.get(this.baseUrl + "file/get-documentation-pdf", { responseType: 'text' });
  }

  uploadDll(file: File, username: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('username', username);
    return this.http.post(this.baseUrl + "file/upload-dll", formData);
  }
}
