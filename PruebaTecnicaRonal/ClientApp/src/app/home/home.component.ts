import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Security } from 'src/app/services/security';
import { Data } from '../Services/data';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  title = 'app';



  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private securityService: Security,
    private dataService: Data,

    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.securityService.LogOff();
  }

  checkoutForm = this.formBuilder.group({
    userName: '',
    password: ''
  });



  onSubmit(event: Event): void {
    event.preventDefault();
    console.log(this.checkoutForm.value);

    this.dataService.post('/login', this.checkoutForm.value).subscribe(res => {
      const token = res.body;
      console.log('token', token);
      this.securityService.SetAuthData(token);
      this.router.navigate(['/fetch-data']);
    }, err => {
      console.log('Error en el login', err);
    });

    this.checkoutForm.reset();
  }
}



