import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { BookService } from '../services/book.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Book, PurchaseType } from '../models/book';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { AuthorService } from '../../authors/services/author.service';
import { SubjectService } from '../../subjects/services/subject.service';
import { Author } from '../../authors/models/author';
import { Subject } from '../../subjects/models/subject';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCard,
    MatCardContent,
    MatCardActions,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
    MatSnackBarModule,
    MatSelectModule,
    MatChipsModule,
    MatIconModule,
    MatDividerModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  bookForm: FormGroup;

  authors = signal<Author[]>([]);
  subjects = signal<Subject[]>([]);

  purchaseTypes = [
    { value: PurchaseType.PhysicalStore, label: 'Loja Física' },
    { value: PurchaseType.Online, label: 'Online' },
    { value: PurchaseType.SelfService, label: 'Autoatendimento' },
    { value: PurchaseType.Event, label: 'Evento' },
    { value: PurchaseType.Subscription, label: 'Assinatura' },
    { value: PurchaseType.EBook, label: 'E-Book' },
    { value: PurchaseType.Audiobook, label: 'Audiobook' },
    { value: PurchaseType.Rental, label: 'Aluguel' }
  ];

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private authorService: AuthorService,
    private subjectService: SubjectService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.bookForm = this.createBookForm();

    this.loadAuthors();
    this.loadSubjects();
  }

  createBookForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required]],
      publisher: ['', [Validators.required]],
      edition: ['', [Validators.required, Validators.min(1)]],
      publicationYear: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      prices: this.fb.array([this.createPriceGroup()]),
      authorsId: [[], [Validators.required]],
      subjectsId: [[], [Validators.required]]
    });
  }

  createPriceGroup(): FormGroup {
    return this.fb.group({
      price: ['', [Validators.required, Validators.min(0.01)]],
      purchaseType: ['', [Validators.required]]
    });
  }

  get pricesArray(): FormArray {
    return this.bookForm.get('prices') as FormArray;
  }

  addPrice(): void {
    this.pricesArray.push(this.createPriceGroup());
  }

  removePrice(index: number): void {
    if (this.pricesArray.length > 1) {
      this.pricesArray.removeAt(index);
    }
  }

  loadAuthors(): void {
    this.authorService.getAuthors()
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: (authors) => {
          this.authors.set(authors);
        },
        error: (error) => {
          this.snackBar.open(`Erro ao carregar autores: ${error.message}`, 'Fechar', {
            duration: 5000
          });
        }
      });
  }

  loadSubjects(): void {
    this.subjectService.getSubjects()
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: (subjects) => {
          this.subjects.set(subjects);
        },
        error: (error) => {
          this.snackBar.open(`Erro ao carregar disciplinas: ${error.message}`, 'Fechar', {
            duration: 5000
          });
        }
      });
  }

  onSubmit(): void {
    if (this.bookForm.valid) {
      const formValues = this.bookForm.value;

      // Criar objeto para enviar ao serviço
      const bookData = {
        title: formValues.title,
        publisher: formValues.publisher,
        edition: formValues.edition,
        publicationYear: formValues.publicationYear,
        prices: formValues.prices,
        authorsId: formValues.authorsId,
        subjectsId: formValues.subjectsId
      };

      console.log('Form data to send:', bookData);

      this.bookService.addBook(bookData)
        .subscribe({
          next: () => {
            this.snackBar.open('Livro cadastrado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/books']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao cadastrar livro: ${error.message}`, 'Fechar', {
              duration: 5000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
          }
        });
    } else {
      this.markFormGroupTouched(this.bookForm);
      this.snackBar.open('Por favor, corrija os erros no formulário antes de enviar.', 'Fechar', {
        duration: 5000
      });
    }
  }


  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else if (control instanceof FormArray) {
        control.controls.forEach(ctrl => {
          if (ctrl instanceof FormGroup) {
            this.markFormGroupTouched(ctrl);
          } else {
            ctrl.markAsTouched();
          }
        });
      }
    });
  }
}
