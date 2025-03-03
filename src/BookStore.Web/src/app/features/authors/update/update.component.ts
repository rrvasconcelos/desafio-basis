import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthorService } from '../services/author.service';
import { Author } from '../models/author';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-update',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatCard,
    MatCardContent,
    MatCardActions,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
    MatSnackBarModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export class UpdateComponent {
  readonly formGroup = new FormGroup({
    nome: new FormControl('', [Validators.required])
  });

  errorMessage = signal('');
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

    this.formGroup.get('nome')?.valueChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());

    this.formGroup.get('nome')?.statusChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());
  }

  loadAuthor(id: number) {
    this.service.getById(id)
      .subscribe({
        next: (author) => {
          this.formGroup.get('nome')?.setValue(author.name);
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

  updateErrorMessage() {
    const nomeControl = this.formGroup.get('nome');
    if (nomeControl?.hasError('required')) {
      this.errorMessage.set('esse campo é obrigatório');
    } else {
      this.errorMessage.set('');
    }
  }

  onSubmit() {
    if (this.formGroup.valid) {
      const author: Author = {
        id: this.authorId(),
        name: this.formGroup.get('nome')?.value || ''
      };

      this.service.updateAuthor(author)
        .subscribe({
          next: () => {
            this.snackBar.open('Autor atualizado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/authors']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao atualizar autor: ${error.message}`, 'Fechar', {
              duration: 5000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
          }
        });
    } else {
      this.formGroup.markAllAsTouched();
      this.updateErrorMessage();
    }
  }
}
