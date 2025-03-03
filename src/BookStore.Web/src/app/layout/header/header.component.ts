import { Component } from '@angular/core';
import {MatIcon, MatIconModule} from "@angular/material/icon";
import {MatButtonModule, MatIconButton} from "@angular/material/button";
import {MatMenu, MatMenuItem, MatMenuModule} from "@angular/material/menu";
import {MatToolbar, MatToolbarModule} from "@angular/material/toolbar";
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-header',
    imports: [
      MatToolbarModule,
      MatButtonModule,
      MatIconModule,
      MatMenuModule,
      RouterLink
    ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

}
