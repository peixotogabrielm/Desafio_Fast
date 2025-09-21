import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Ata, Workshop } from '../../models';
import { MockDataService } from '../../services/mock-data.service';

@Component({
  selector: 'app-workshop-details',
  templateUrl: './workshop-details.component.html',
  styleUrls: ['./workshop-details.component.scss']
})
export class WorkshopDetailsComponent implements OnInit {
  ata$!: Observable<Ata | undefined>;
  workshopId!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private mockDataService: MockDataService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.workshopId = +params['id'];
      this.carregarDetalhesWorkshop();
    });
  }

  carregarDetalhesWorkshop(): void {
    this.ata$ = this.mockDataService.obterAtas().pipe(
      switchMap((atas: Ata[]) => {
        const ata = atas.find((a: Ata) => a.workshop.id === this.workshopId);
        return new Observable<Ata | undefined>(observer => {
          observer.next(ata);
          observer.complete();
        });
      })
    );
  }

  voltarParaLista(): void {
    this.router.navigate(['/atas']);
  }

  formatarData(data: Date): string {
    return new Date(data).toLocaleDateString('pt-BR');
  }
}
