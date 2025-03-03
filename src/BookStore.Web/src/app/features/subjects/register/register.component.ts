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
import { SubjectService } from '../services/subject.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject } from '../models/subject';

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
    description: new FormControl('', [Validators.required])
  });

  errorMessage = signal('');

  constructor(
    private service: SubjectService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.formGroup.get('description')?.valueChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());

    this.formGroup.get('description')?.statusChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());
  }

  updateErrorMessage() {
    const descriptionControl = this.formGroup.get('description');
    if (descriptionControl?.hasError('required')) {
      this.errorMessage.set('esse campo é obrigatório');
    } else {
      this.errorMessage.set('');
    }
  }

  onSubmit() {
    if (this.formGroup.valid) {
      const subject: Subject = {
        id: 0,
        description: this.formGroup.get('description')?.value || ''
      };

      this.service.addSubject(subject)
        .subscribe({
          next: () => {
            this.snackBar.open('Assunto cadastrado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/subjects']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao cadastrar Assunto: ${error.message}`, 'Fechar', {
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
