import {Component, signal} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatCard, MatCardActions, MatCardContent} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {MatButtonModule} from '@angular/material/button';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {BookService} from '../services/book.service';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';
import {Book, BookPrice, BookRequest, Price, PurchaseType} from '../models/book';
import {MatSelectModule} from '@angular/material/select';
import {MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import {AuthorService} from '../../authors/services/author.service';
import {SubjectService} from '../../subjects/services/subject.service';
import {Author} from '../../authors/models/author';
import {Subject} from '../../subjects/models/subject';
import {MatDividerModule} from '@angular/material/divider';
import {forkJoin} from 'rxjs';

@Component({
  selector: 'app-update',
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
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export class UpdateComponent {
  bookForm: FormGroup;
  bookId = signal<number>(0);

  authors = signal<Author[]>([]);
  subjects = signal<Subject[]>([]);
  bookData = signal<Book | null>(null);

  purchaseTypes = [
    {value: PurchaseType.PhysicalStore, label: 'Loja Física'},
    {value: PurchaseType.Online, label: 'Online'},
    {value: PurchaseType.SelfService, label: 'Autoatendimento'},
    {value: PurchaseType.Event, label: 'Evento'},
    {value: PurchaseType.Subscription, label: 'Assinatura'},
    {value: PurchaseType.EBook, label: 'E-Book'},
    {value: PurchaseType.Audiobook, label: 'Audiobook'},
    {value: PurchaseType.Rental, label: 'Aluguel'}
  ];

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private authorService: AuthorService,
    private subjectService: SubjectService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.bookForm = this.createBookForm();

    this.route.paramMap
      .pipe(takeUntilDestroyed())
      .subscribe(params => {
        const id = params.get('id');
        if (id) {
          this.bookId.set(+id);
          this.loadAllData(+id);
        }
      });
  }

  createBookForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required]],
      publisher: ['', [Validators.required]],
      edition: ['', [Validators.required, Validators.min(1)]],
      publicationYear: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      prices: this.fb.array([]),
      authorsId: [[], [Validators.required]],
      subjectsId: [[], [Validators.required]]
    });
  }

  createPriceGroup(price?: BookPrice | Price): FormGroup {
    let priceValue = 0;

    if (price) {
      if ('value' in price && price.value !== undefined) {
        priceValue = price.value;
      } else if ('price' in price && price.price !== undefined) {
        priceValue = price.price;
      }
    }

    return this.fb.group({
      price: [priceValue, [Validators.required, Validators.min(0.01)]],
      purchaseType: [price?.purchaseType || '', [Validators.required]]
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

  loadAllData(id: number): void {
    forkJoin({
      book: this.bookService.getById(id),
      authors: this.authorService.getAuthors(),
      subjects: this.subjectService.getSubjects()
    })
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: (data) => {
          console.log('Data loaded:', data);

          this.authors.set(data.authors);
          this.subjects.set(data.subjects);
          this.bookData.set(data.book);

          this.populateForm(data.book);
        },
        error: (error) => {
          this.snackBar.open(`Erro ao carregar dados: ${error.message}`, 'Fechar', {
            duration: 5000,
            horizontalPosition: 'end',
            verticalPosition: 'top'
          });
          this.router.navigate(['/books']);
        }
      });
  }

  populateForm(book: Book): void {
    console.log('Populating form with book:', book);

    while (this.pricesArray.length) {
      this.pricesArray.removeAt(0);
    }

    if (book.bookPrices && book.bookPrices.length > 0) {
      book.bookPrices.forEach(priceItem => {
        const priceValue = priceItem.value !== undefined ? priceItem.value :
          (priceItem.price !== undefined ? priceItem.price : 0);

        const price: Price = {
          price: priceValue,
          purchaseType: priceItem.purchaseType
        };
        this.pricesArray.push(this.createPriceGroup(price));
      });
    } else {
      this.addPrice();
    }

    const availableAuthorIds = this.authors().map(a => a.id);
    const availableSubjectIds = this.subjects().map(s => s.id);

    console.log('Book author IDs:', book.authorIds);
    console.log('Available author IDs:', availableAuthorIds);
    console.log('Book subject IDs:', book.subjectsId);
    console.log('Available subject IDs:', availableSubjectIds);

    this.bookForm.patchValue({
      title: book.title,
      publisher: book.publisher,
      edition: book.edition,
      publicationYear: book.publicationYear
    });

    if (book.authorIds && book.authorIds.some(id => availableAuthorIds.includes(id))) {
      this.bookForm.get('authorsId')?.setValue(
        book.authorIds.filter(id => availableAuthorIds.includes(id))
      );
    } else if (availableAuthorIds.length > 0) {
      this.bookForm.get('authorsId')?.setValue([availableAuthorIds[0]]);
      this.snackBar.open('Os autores originais do livro não foram encontrados. Um autor substituto foi selecionado.', 'OK', {
        duration: 5000
      });
    }

    if (book.subjectsId && book.subjectsId.some(id => availableSubjectIds.includes(id))) {
      this.bookForm.get('subjectsId')?.setValue(
        book.subjectsId.filter(id => availableSubjectIds.includes(id))
      );
    } else if (availableSubjectIds.length > 0) {
      this.bookForm.get('subjectsId')?.setValue([availableSubjectIds[0]]);
      this.snackBar.open('As disciplinas originais do livro não foram encontradas. Uma disciplina substituta foi selecionada.', 'OK', {
        duration: 5000
      });
    }

    console.log('Form values after all updates:', this.bookForm.value);
  }

  onSubmit(): void {
    if (this.bookForm.valid) {
      const formValues = this.bookForm.value;

      const bookData: BookRequest = {
        id: this.bookId(),
        title: formValues.title,
        publisher: formValues.publisher,
        edition: formValues.edition,
        publicationYear: formValues.publicationYear,
        prices: formValues.prices,
        authorsId: formValues.authorsId,
        subjectsId: formValues.subjectsId
      };

      console.log('Updating book:', bookData);

      this.bookService.updateBook(bookData)
        .subscribe({
          next: () => {
            this.snackBar.open('Livro atualizado com sucesso!', 'Fechar', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top'
            });
            this.router.navigate(['/books']);
          },
          error: (error) => {
            this.snackBar.open(`Erro ao atualizar livro: ${error.message}`, 'Fechar', {
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
