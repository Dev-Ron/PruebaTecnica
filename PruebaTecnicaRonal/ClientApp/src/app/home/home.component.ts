import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

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


    @Inject('BASE_URL') private baseUrl: string
  ) {

  }

  checkoutForm = this.formBuilder.group({
    userName: '',
    password: ''
  });



  onSubmit(event: Event): void {
    event.preventDefault();
    console.log(this.checkoutForm.value);

    this.http.post('/login', this.checkoutForm.value).subscribe(result => {
    
      localStorage.setItem('JWT', JSON.stringify(result));
      this.router.navigate(['/fetch-data'], { relativeTo: this.route });
    }, error => {
      console.error(error);
      
    });

    this.checkoutForm.reset();
  }
}



