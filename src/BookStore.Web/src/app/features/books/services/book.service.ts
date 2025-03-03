import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {environment} from '../../../../environments/environment.development';
import {Book, BookRequest} from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = `${environment.apiUrl}/books`;

  constructor(private http: HttpClient) {
  }

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  getById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  addBook(bookData: any): Observable<Book> {
    // Criar objeto no formato que o backend espera
    const requestData: BookRequest = {
      title: bookData.title,
      publisher: bookData.publisher,
      edition: bookData.edition,
      publicationYear: bookData.publicationYear,
      prices: bookData.prices || bookData.bookPrices,
      authorsId: bookData.authorsId || bookData.authorIds,
      subjectsId: bookData.subjectsId || []
    };

    console.log('Sending to backend:', requestData);

    return this.http.post<Book>(this.apiUrl, requestData).pipe(
      catchError(this.handleError)
    );
  }

  updateBook(bookData: any): Observable<Book> {
    // Criar objeto no formato que o backend espera
    const requestData: BookRequest = {
      id: bookData.id,
      title: bookData.title,
      publisher: bookData.publisher,
      edition: bookData.edition,
      publicationYear: bookData.publicationYear,
      prices: bookData.prices || bookData.bookPrices,
      authorsId: bookData.authorsId || bookData.authorIds,
      subjectsId: bookData.subjectsId || []
    };

    return this.http.put<Book>(`${this.apiUrl}/${bookData.id}`, requestData).pipe(
      catchError(this.handleError)
    );
  }

  deleteBook(id: number): Observable<Book> {
    return this.http.delete<Book>(`${this.apiUrl}/${id}`).pipe(
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
