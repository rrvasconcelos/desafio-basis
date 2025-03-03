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
import { Router, RouterLink } from '@angular/router';
import { AuthorService } from '../services/author.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Author } from '../models/author';

@Component({
  selector: 'app-register',
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
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  readonly formGroup = new FormGroup({
    nome: new FormControl('', [Validators.required])
  });

  errorMessage = signal('');

  constructor(
    private service: AuthorService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.formGroup.get('nome')?.valueChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());

    this.formGroup.get('nome')?.statusChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());
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
        id: 0,
        name: this.formGroup.get('nome')?.value || ''
      };

      this.service.addAuthor(author)
        .subscribe({
          next: () => {
            this.snackBar.open('Autor cadastrado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/authors']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao cadastrar autor: ${error.message}`, 'Fechar', {
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
