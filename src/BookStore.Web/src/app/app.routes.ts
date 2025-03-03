import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: 'books',
    loadChildren: () => import(`./features/books/books.routes`)
      .then(routes => routes.routes),
  },
  {
    path: 'authors',
    loadChildren: () => import(`./features/authors/authors.routes`)
      .then(routes => routes.routes),
  },
  {
    path: 'subjects',
    loadChildren: () => import(`./features/subjects/subjects.route`)
      .then(routes => routes.routes),
  },
];
