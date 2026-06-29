import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { ChangeDetectorRef, Component } from "@angular/core";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-help",
  imports: [FormsModule,
    CommonModule,
    HttpClientModule],
  templateUrl: "./help.html",
  styleUrl: "./help.scss",
})
export class Help {

  constructor(private http: HttpClient , private cd:ChangeDetectorRef) { }

  searchText = '';

  cards: any[] = [];

  filteredCards: any[] = [];

  selectedCard: any = null;

  ngOnInit(): void {
    this.loadHelpData();
  }

  loadHelpData() {

    this.http.get<any[]>('help.json')
      .subscribe({

        next: (data:any) => {

          this.cards = data;
          this.filteredCards = data;
          this.cd.detectChanges();
        },

        error: (err:any) => {

          console.error("Unable to load Help JSON", err);

        }

      });

  }

  onSearch() {

    const value = this.searchText.toLowerCase().trim();

    if (!value) {

      this.filteredCards = this.cards;
      this.cd.detectChanges();
      return;

    }

    this.filteredCards = this.cards.filter(card =>

      card.title.toLowerCase().includes(value) ||

      card.description.toLowerCase().includes(value) ||

      card.items.some((item: any) =>

        item.title.toLowerCase().includes(value) ||

        item.description.toLowerCase().includes(value)

      )

    );

  }

  selectCard(card: any) {

    this.selectedCard = card;

  }

  closeCard() {

    this.selectedCard = null;

  }


}
