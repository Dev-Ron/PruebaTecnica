import { Component, Inject } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements HttpInterceptor {
  public autores: autores[];
  cols: any[];
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
    const token = localStorage.getItem('JWT');
    http.get<autores[]>(baseUrl + 'weatherforecast', { responseType: 'json', headers: { Authorization: `Bearer ${token}` } }).subscribe(result => {
      console.log(result);
      this.autores = result;
    }, error => console.error(error));

    this.cols = [
      { field: 'title', header: 'Titulo' },
      { field: 'firstName', header: 'Nombre autor' },
      { field: 'lastName', header: 'Apellido autor' },
      { field: 'pageCount', header: 'Número de paginas' },
      { field: 'description', header: 'Descripción' },
      { field: 'publishDate', header: 'Fecha de publicación' }
    ];
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('JWT');
    req = req.clone({
      url: req.url,
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next.handle(req);
  }


  onSincronizar() {
    const token = localStorage.getItem('JWT');
    this.http.post<autores[]>(this.baseUrl + 'weatherforecast/Sincronizar',
      { responseType: 'json', headers: { Authorization: `Bearer ${token}` } }).subscribe(result => {
      this.autores = result;
    }, error => {
      console.error(error);
    });
  }

  onEliminar() {
    const token = localStorage.getItem('JWT');
    this.http.delete(this.baseUrl + 'weatherforecast/Eliminar').subscribe(result => {
      this.autores = [];
      }, error => {
        console.error(error);
      });
  }

  checkoutFormFiltro = this.formBuilder.group({
    Campo: '',
    Busqueda: ''
  });

  onSubmit(event: Event): void {
    const token = localStorage.getItem('JWT');
    event.preventDefault();
    console.log(this.checkoutFormFiltro.value);
    this.http.post<autores[]>(this.baseUrl + 'weatherforecast/ConsultarFiltro', this.checkoutFormFiltro.value, { responseType: 'json', headers: { Authorization: `Bearer ${token}` } }).subscribe(result => {
      this.autores = result;
    }, error => {
      console.error(error);
    });
  }

}

interface autores {
  libro: libro;
  firstName: string;
  lastName: string;
}

interface libro {
  title: string;
  description: string;
  pageCount: number;
  excerpt: string;
  publishDate: Date;
}




