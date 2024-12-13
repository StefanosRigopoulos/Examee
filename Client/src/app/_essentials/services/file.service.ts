import { HttpClient, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) {}

  getFileDllURL() {
    return this.http.get(this.baseUrl + "download/get_dll", { responseType: 'text' });
  }

  uploadDll(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return new Observable((observer) => {
      this.http
        .post(this.baseUrl + "upload/dll", formData, {
          reportProgress: true,
          observe: 'events',
        })
        .subscribe({
          next: (event: any) => {
            if (event.type === HttpEventType.UploadProgress) {
              const progress = Math.round((event.loaded / event.total) * 100);
              observer.next({ status: 'progress', progress });
            } else if (event.type === HttpEventType.Response) {
              observer.next({ status: 'complete', body: event.body });
              observer.complete();
            }
          },
          error: (error) => {
            observer.error(error);
          },
        });
    });
  }
}
