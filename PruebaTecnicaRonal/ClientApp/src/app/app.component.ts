import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {

  }


  checkoutForm = this.formBuilder.group({
    userName: '',
    password: ''
  });

  onSubmit(event: Event): void {
    event.preventDefault();
    console.log(this.checkoutForm.value);

    this.http.post('https://localhost:44325/login', this.checkoutForm.value).subscribe(result => {
      console.log(result)
    }, error => console.error(error));

    this.checkoutForm.reset();
  }
}



