import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Author} from '../models/author';
import {catchError, Observable, throwError} from 'rxjs';
import {environment} from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = `${environment.apiUrl}/authors`;

  constructor(private http: HttpClient) {
  }

  getAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  addAuthor(author: Author): Observable<Author> {
    return this.http.post<Author>(this.apiUrl, author).pipe(
      catchError(this.handleError)
    );
  }

  getById(id: number): Observable<Author> {
    return this.http.get<Author>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  updateAuthor(author: Author): Observable<Author> {
    return this.http.put<Author>(`${this.apiUrl}/${author.id}`, author).pipe(
      catchError(this.handleError)
    );
  }

  deleteAuthor(id: number): Observable<Author> {
    return this.http.delete<Author>(`${this.apiUrl}/${id}`).pipe(
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
