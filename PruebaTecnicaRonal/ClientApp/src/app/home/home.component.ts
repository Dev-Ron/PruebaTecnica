import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Security } from '../Services/securityService';
import { Data } from '../Services/dataService';
import notify from 'devextreme/ui/notify';
import { ToastrService } from 'ngx-toastr';

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
    private toastr: ToastrService,

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
    document.getElementById("SpanLoging").style.display = "inline-block";

    this.dataService.post('/login', this.checkoutForm.value).subscribe(res => {
      if (res.ok) {
        this.checkoutForm.reset();
          this.toastr.success('Usuario Logueado');
        

        const token = res.body;
        this.securityService.SetAuthData(token);
        this.router.navigate(['/fetch-data']);
      } else {
        notify({
          message: res.body,
          width: 450
        },
          "error",
          2000);
      }
      document.getElementById("SpanLoging").style.display = "none";
     

    }, err => {

        console.log(err);

        notify({
          message: err.error,
          width: 450
        },
          "error",
          2000);
        document.getElementById("SpanLoging").style.display = "none";
    });
   
    
   
  }
}



