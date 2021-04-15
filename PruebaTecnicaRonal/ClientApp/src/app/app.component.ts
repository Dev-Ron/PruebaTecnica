import { Component, Input, OnDestroy } from '@angular/core';

import { Security } from './Services/securityService';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent implements OnDestroy {
  IsAuthenticated = false;
  title = 'Prueba Tecnica 2.0';
  private subsAuth$: Subscription;

  constructor(
    private securityService: Security
  ) {
    this.IsAuthenticated = this.securityService.IsAuthorized;

    // Nos suscribimos al observador de los eventos de auth.
    this.subsAuth$ = this.securityService.authChallenge$.subscribe(
      (isAuth) => {
        this.IsAuthenticated = isAuth;
      });
  }

  ngOnDestroy() {
    if (this.subsAuth$) {
      this.subsAuth$.unsubscribe();
    }
  }


}
