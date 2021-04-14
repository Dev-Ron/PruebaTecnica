import { Component, Inject, NgModule } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { exportDataGrid } from "devextreme/exporter";
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import { locale, loadMessages } from "devextreme/localization";
import esMessages from "devextreme/localization/messages/es.json";
import notify from 'devextreme/ui/notify';

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
  saleAmountHeaderFilter: any;
  applyFilterTypes: any;
  currentFilter: any;
  showFilterRow: boolean;
  showHeaderFilter: boolean;

  cols: any[];
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
   
   
      this.onCargarGrid();


    
    loadMessages(esMessages);
    locale(navigator.language);

  
    this.showFilterRow = true;
    this.showHeaderFilter = true;
    this.applyFilterTypes = [{
      key: "auto",
      name: "Immediately"
    }, {
      key: "onClick",
      name: "On Button Click"
    }];
  }

  onExporting(e) {
    const workbook = new Workbook.Workbook();
    const worksheet = workbook.addWorksheet('Employees');

    exportDataGrid({
      component: e.component,
      worksheet: worksheet,
      autoFilterEnabled: true
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
      });
    });
    e.cancel = true;
  }

  onSincronizar() {
    document.getElementById("SincronizarCargando").style.display = "block";
    this.http.post<autores[]>(this.baseUrl + 'libros/Sincronizar', { responseType: 'json' }).subscribe(result => {
      document.getElementById("SincronizarCargando").style.display = "none";
      this.onCargarGrid();
    }, error => {
        document.getElementById("SincronizarCargando").style.display = "none";
        document.getElementById("CargandoGrid").style.display = "inline-block";
        this.onCargarGrid();
    });
  }

  onEliminar() {
    document.getElementById("EliminarCargando").style.display = "block";
    this.http.delete(this.baseUrl + 'libros/Eliminar').subscribe(result => {
      document.getElementById("EliminarCargando").style.display = "none";
      this.onCargarGrid();
      }, error => {
        console.error(error);
        document.getElementById("EliminarCargando").style.display = "none";
        this.onCargarGrid();
      });
  }

  onCargarGrid() {
    
    this.http.get<autores[]>(this.baseUrl + 'libros').subscribe(result => {
      this.autores = result;
      document.getElementById("CargandoGrid").style.display = "none";
    }, error => console.error(error));
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




