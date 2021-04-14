import { Component, Inject, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap  } from '@angular/router';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { Security } from './services/security';
import { Subscription } from 'rxjs';
import { Data } from 'src/app/services/data';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent implements OnDestroy {
  IsAuthenticated = false;
  title = 'Prueba Tecnica 2.0';
  private subsAuth$: Subscription;

  constructor(
    private securityService: Security,
    private dataService: Data,
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
