import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, Router } from '@angular/router';
import { SubjectService } from '../services/subject.service';
import { Subject } from '../models/subject';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

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
    MatSnackBarModule
  ],
  templateUrl: './get-all.component.html',
  styleUrl: './get-all.component.scss'
})
export class GetAllComponent implements OnInit {
  dataSource = signal<Subject[]>([]);
  displayedColumns: string[] = ['Id', 'description', 'Ações'];

  constructor(
    private service: SubjectService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.service.getSubjects().subscribe({
      next: (subjects) => {
        this.dataSource.set(subjects);
      },
      error: (error) => {
        this.snackBar.open(`Erro ao carregar assuntos: ${error.message}`, 'Fechar', {
          duration: 5000,
          horizontalPosition: 'end',
          verticalPosition: 'top'
        });
      }
    });
  }

  editSubject(subject: Subject): void {
    this.router.navigate(['/subjects/update', subject.id]);
  }

  deleteSubject(subject: Subject): void {
    this.router.navigate(['/subjects/delete', subject.id]);
  }
}
