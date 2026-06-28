import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { Restservice } from '../../services/restservice/restservice';
import { LoaderService } from '../../services/loaderService';
import { AlertService } from '../../common/alert/alert.service';
import { CommonModule } from '@angular/common';

export interface StockResult {
  description: string;
  displaySymbol: string;
  symbol: string;
  type: string;
}
@Component({
  selector: 'app-stock-search',
  imports: [MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    //MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    FormsModule,CommonModule],
  templateUrl: './stock-search.html',
  styleUrl: './stock-search.scss',
})
export class StockSearch {

  searchText = '';

  hasSearched = false;

  displayedColumns = [
    'displaySymbol',
    'description',
    'type',
    'actions'
  ];

  dataSource = new MatTableDataSource<StockResult>();

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  @ViewChild(MatSort)
  sort!: MatSort;

  constructor(private _restService: Restservice, private cd: ChangeDetectorRef, private loader: LoaderService, private alertService: AlertService) { }


  searchStocks() {

    if (!this.searchText?.trim()) return;

    this.hasSearched = true;
    this.loader.show();
    this._restService
      .searchStocks(this.searchText)
      .subscribe(res => {
        this.dataSource.data = res;

        this.dataSource.paginator = this.paginator;

        this.dataSource.sort = this.sort;
        this.cd.detectChanges();
        console.log(this.dataSource);
        this.loader.hide();
      });

  }

  applyFilter(event: Event) {

    const filterValue =
      (event.target as HTMLInputElement)
        .value;

    this.dataSource.filter =
      filterValue.trim()
        .toLowerCase();
  }

  viewDetails(row: StockResult) {

    console.log(row);
    this._restService.GetStockDetails(row?.symbol).subscribe((res:any)=>{
      console.log(res);
    })
  }
}
