import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public libros: Libro[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Libro[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.libros = result;
    }, error => console.error(error));
  }
}

interface Libro {
  title: string;
  description: string;
  pageCount: number;
  excerpt: string;
  publishDate: Date;
}
