import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { Subject } from '../models/subject';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private apiUrl = `${environment.apiUrl}/subjects`;

  constructor(private http: HttpClient) { }

  getSubjects(): Observable<Subject[]> {
    return this.http.get<Subject[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  addSubject(subject: Subject): Observable<Subject> {
    return this.http.post<Subject>(this.apiUrl, subject).pipe(
      catchError(this.handleError)
    );
  }

  getById(id: number): Observable<Subject> {
    return this.http.get<Subject>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  updateSubject(subject: Subject): Observable<Subject> {
    return this.http.put<Subject>(`${this.apiUrl}/${subject.id}`, subject).pipe(
      catchError(this.handleError)
    );
  }

  deleteSubject(id: number): Observable<Subject> {
    return this.http.delete<Subject>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Ocorreu um erro desconhecido!';

    if (error.error instanceof ErrorEvent) {
      // Erro do lado do cliente
      errorMessage = `Erro: ${error.error.message}`;
    } else {
      // Erro retornado pelo backend
      if (error.error && typeof error.error === 'object') {
        // Verifica se existe a propriedade detail no objeto de erro
        if (error.error.detail) {
          errorMessage = error.error.detail;
        } else if (error.error.title) {
          errorMessage = error.error.title;
        } else {
          errorMessage = `Erro ${error.status}: ${error.statusText}`;
        }
      } else {
        errorMessage = `Erro ${error.status}: ${error.statusText || error.message}`;
      }
    }

    console.error('API Error:', error);
    const err = new Error(errorMessage);
    return throwError(() => err);
  }
}
