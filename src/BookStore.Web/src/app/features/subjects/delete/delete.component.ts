import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SubjectService } from '../services/subject.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Subject } from '../models/subject';

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
  subject = signal<Subject | null>(null);
  subjectId = signal<number>(0);

  constructor(
    private service: SubjectService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.route.paramMap
      .pipe(takeUntilDestroyed())
      .subscribe(params => {
        const id = params.get('id');
        if (id) {
          this.subjectId.set(+id);
          this.loadSubject(+id);
        }
      });
  }

  loadSubject(id: number) {
    this.service.getById(id)
      .subscribe({
        next: (subject) => {
          this.subject.set(subject);
        },
        error: (error) => {
          this.snackBar.open(`Erro ao carregar assuntos: ${error.message}`, 'Fechar', {
            duration: 5000,
            horizontalPosition: 'end',
            verticalPosition: 'top'
          });
          this.router.navigate(['/subjects']);
        }
      });
  }

  confirmDelete() {
    if (this.subjectId()) {
      this.service.deleteSubject(this.subjectId())
        .subscribe({
          next: () => {
            this.snackBar.open('Assunto excluído com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/subjects']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao excluir Assunto: ${error.message}`, 'Fechar', {
              duration: 5000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
          }
        });
    }
  }

  cancelDelete() {
    this.router.navigate(['/subjects']);
  }
}
