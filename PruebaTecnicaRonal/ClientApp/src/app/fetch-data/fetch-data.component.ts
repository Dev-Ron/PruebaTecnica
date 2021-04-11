import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';



@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public autores: autores[];

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
    http.get<autores[]>(baseUrl + 'weatherforecast').subscribe(result => {
      console.log(result);
      this.autores = result;
    }, error => console.error(error));
  }


  onSincronizar() {
    this.http.post<autores[]>(this.baseUrl + 'weatherforecast/Sincronizar', { responseType: 'json' }).subscribe(result => {
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
