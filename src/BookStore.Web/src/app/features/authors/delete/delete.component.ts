import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthorService } from '../services/author.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Author } from '../models/author';

@Component({
  selector: 'app-delete',
  standalone: true,
  imports: [
    CommonModule,
    MatCard,
    MatCardContent,
    MatCardActions,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './delete.component.html',
  styleUrl: './delete.component.scss'
})
export class DeleteComponent {
  author = signal<Author | null>(null);
  authorId = signal<number>(0);

  constructor(
    private service: AuthorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.route.paramMap
      .pipe(takeUntilDestroyed())
      .subscribe(params => {
        const id = params.get('id');
        if (id) {
          this.authorId.set(+id);
          this.loadAuthor(+id);
        }
      });
  }

  loadAuthor(id: number) {
    this.service.getById(id)
      .subscribe({
        next: (author) => {
          this.author.set(author);
        },
        error: (error) => {
          this.snackBar.open(`Erro ao carregar autor: ${error.message}`, 'Fechar', {
            duration: 5000,
            horizontalPosition: 'end',
            verticalPosition: 'top'
          });
          this.router.navigate(['/authors']);
        }
      });
  }

  confirmDelete() {
    if (this.authorId()) {
      this.service.deleteAuthor(this.authorId())
        .subscribe({
          next: () => {
            this.snackBar.open('Autor excluído com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/authors']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao excluir autor: ${error.message}`, 'Fechar', {
              duration: 5000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
          }
        });
    }
  }

  cancelDelete() {
    this.router.navigate(['/authors']);
  }
}
