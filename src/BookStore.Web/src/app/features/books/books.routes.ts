import { Routes } from '@angular/router';
import { BooksComponent } from './books.component';

export const routes: Routes = [
  {
    path: '', component: BooksComponent, children: [
      {
        path: '',
        loadComponent: () => import(`./get-all/get-all.component`)
          .then(mod => mod.GetAllComponent)
      },
      {
        path: 'register',
        loadComponent: () => import(`./register/register.component`)
          .then(mod => mod.RegisterComponent)
      },
      {
        path: 'update/:id',
        loadComponent: () => import(`./update/update.component`)
          .then(mod => mod.UpdateComponent)
      },
      {
        path: '**',
        loadComponent: () => import(`./get-all/get-all.component`)
          .then(mod => mod.GetAllComponent)
      },
    ]
  },
];
