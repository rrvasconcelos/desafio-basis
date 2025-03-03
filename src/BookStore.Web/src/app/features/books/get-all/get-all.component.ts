import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, Router } from '@angular/router';
import { BookService } from '../services/book.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthorService } from '../../authors/services/author.service';
import { SubjectService } from '../../subjects/services/subject.service';
import { forkJoin } from 'rxjs';
import { Author } from '../../authors/models/author';
import { Subject } from '../../subjects/models/subject';

interface BookResponse {
  id: number;
  authorIds: number[];
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  bookPrices: any[];
  createdAt: string;
  active: boolean;
}

@Component({
  selector: 'app-get-all',
  standalone: true,
  imports: [
    CommonModule,
    MatCard,
    MatCardContent,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    MatSnackBarModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './get-all.component.html',
  styleUrl: './get-all.component.scss'
})
export class GetAllComponent implements OnInit {
  books = signal<BookResponse[]>([]);
  authors = signal<Author[]>([]);
  subjects = signal<Subject[]>([]);
  isLoading = signal<boolean>(true);

  displayedColumns: string[] = ['id', 'title', 'publisher', 'edition', 'publicationYear', 'authors', 'actions'];

  constructor(
    private bookService: BookService,
    private authorService: AuthorService,
    private subjectService: SubjectService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading.set(true);

    forkJoin({
      books: this.bookService.getBooks(),
      authors: this.authorService.getAuthors(),
      subjects: this.subjectService.getSubjects()
    }).subscribe({
      next: (result) => {
        this.authors.set(result.authors);
        this.subjects.set(result.subjects);
        this.books.set(result.books as BookResponse[]);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.snackBar.open(`Erro ao carregar dados: ${error.message}`, 'Fechar', {
          duration: 5000,
          horizontalPosition: 'end',
          verticalPosition: 'top'
        });
        this.isLoading.set(false);
      }
    });
  }

  getAuthorName(authorId: any): string {
    const id = Number(authorId);
    const author = this.authors().find(a => a.id === id);
    return author?.name || 'Autor desconhecido';
  }

  getAuthorNames(authorIds: any[]): string {
    return authorIds.map(id => this.getAuthorName(id)).join(', ');
  }


  editBook(book: BookResponse): void {
    this.router.navigate(['/books/update', book.id]);
  }

  deleteBook(book: BookResponse): void {
    this.router.navigate(['/books/delete', book.id]);
  }
}
