import { Component, Inject, NgModule } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';



@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

export class FetchDataComponent {
  public autores: autores[];
  dataSource: any;
  filterValue: Array<any>;
  customOperations: Array<any>;
  popupPosition: any;
  cols: any[];
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
  


    http.get<autores[]>(baseUrl + 'libros' ).subscribe(result => {
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
  
  onSincronizar() {
    const token = localStorage.getItem('JWT');
    this.http.post<autores[]>(this.baseUrl + 'libros/Sincronizar', { responseType: 'json' }).subscribe(result => {
      this.autores = result;
    }, error => {
      console.error(error);
    });
  }

  onEliminar() {
    const token = localStorage.getItem('JWT');
    this.http.delete(this.baseUrl + 'libros/Eliminar').subscribe(result => {
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
    this.http.post<autores[]>(this.baseUrl + 'libros/ConsultarFiltro', this.checkoutFormFiltro.value ).subscribe(result => {
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




