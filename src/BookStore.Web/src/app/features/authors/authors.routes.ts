import {Routes} from '@angular/router';
import {AuthorsComponent} from './authors.component';

export const routes: Routes = [
  {
    path: '', component: AuthorsComponent, children: [
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
        path: 'delete/:id',
        loadComponent: () => import(`./delete/delete.component`)
          .then(mod => mod.DeleteComponent)
      },
      {
        path: '**',
        loadComponent: () => import(`./get-all/get-all.component`)
          .then(mod => mod.GetAllComponent)
      },
    ]
  },
];
