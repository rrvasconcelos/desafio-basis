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
import { SubjectService } from '../services/subject.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject } from '../models/subject';

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
    description: new FormControl('', [Validators.required])
  });

  errorMessage = signal('');
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

    this.formGroup.get('description')?.valueChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());

    this.formGroup.get('description')?.statusChanges
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateErrorMessage());
  }

  loadSubject(id: number) {
    this.service.getById(id)
      .subscribe({
        next: (subject) => {
          this.formGroup.get('description')?.setValue(subject.description);
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
        id: this.subjectId(),
        description: this.formGroup.get('description')?.value || ''
      };

      this.service.updateSubject(subject)
        .subscribe({
          next: () => {
            this.snackBar.open('Assunto atualizado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/subjects']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao atualizar assunto: ${error.message}`, 'Fechar', {
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
