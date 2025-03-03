import {Component, OnInit} from '@angular/core';

import {MatCardModule} from '@angular/material/card';

import {AuthorService} from '../services/author.service';
import {Author} from '../models/author';
import {MatTableModule} from '@angular/material/table';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {RouterLink} from '@angular/router';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-get-all',
  imports: [MatCardModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink, NgIf],
  templateUrl: './get-all.component.html',
  styleUrl: './get-all.component.scss'
})
export class GetAllComponent implements OnInit {
  authors: Author[] = [];
  errorMessage: string = '';
  dataSource  : Author[] = [];
  displayedColumns: string[] = ['Id', 'Nome' , 'Ações'];

  constructor(private authorService: AuthorService) {

  }

  ngOnInit(): void {
    this.authorService.getAuthors().subscribe({
      next: (data) => {
        this.dataSource  = data;

        console.log(this.authors);
      },
      error: (err) => (this.errorMessage = err),
    });
  }

  deleteAuthor(author: Author): void {
    console.log('Excluir autor:', author);
  }
}
