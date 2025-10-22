import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, combineLatest } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Ata } from '../../models';
import { MockDataService } from '../../services/mock-data.service';

@Component({
  selector: 'app-atas-list',
  templateUrl: './atas-list.component.html',
  styleUrls: ['./atas-list.component.scss']
})
export class AtasListComponent implements OnInit {
  filtroForm: FormGroup;
  atas$!: Observable<Ata[]>;
  todasAtas: Ata[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private mockDataService: MockDataService
  ) {
    this.filtroForm = this.fb.group({
      colaborador: [''],
      workshop: [''],
      data: ['']
    });
  }

  ngOnInit(): void {
    this.carregarAtas();
  }

  carregarAtas(): void {
    this.mockDataService.obterAtas().subscribe({
      next: (atas: Ata[]) => {
        this.todasAtas = atas;

        this.configurarFiltros();
      },
      error: (error: any) => {
        console.error('❌ Erro ao carregar atas:', error);
      }
    });
  }

  configurarFiltros(): void {
    this.atas$ = combineLatest([
      this.filtroForm.get('colaborador')!.valueChanges.pipe(startWith('')),
      this.filtroForm.get('workshop')!.valueChanges.pipe(startWith('')),
      this.filtroForm.get('data')!.valueChanges.pipe(startWith(''))
    ]).pipe(
      map(([colaborador, workshop, data]) => {
        return this.filtrarAtas(colaborador, workshop, data);
      })
    );
  }

  filtrarAtas(colaborador: string, workshop: string, data: string): Ata[] {
    let atasFiltradas = [...this.todasAtas];

    if (colaborador) {
      atasFiltradas = atasFiltradas.filter(ata =>
        ata.colaboradores.some(col =>
          col.nome.toLowerCase().includes(colaborador.toLowerCase())
        )
      );
    }

    if (workshop) {
      atasFiltradas = atasFiltradas.filter(ata =>
        ata.workshop.nome.toLowerCase().includes(workshop.toLowerCase())
      );
    }

    if (data && data.trim() !== '') {
      atasFiltradas = atasFiltradas.filter(ata => {
        const dataWorkshop = new Date(ata.workshop.dataRealizacao);
        const ano = dataWorkshop.getFullYear();
        const mes = String(dataWorkshop.getMonth() + 1).padStart(2, '0');
        const dia = String(dataWorkshop.getDate()).padStart(2, '0');
        const dataWorkshopFormatada = `${ano}-${mes}-${dia}`;


        const corresponde = dataWorkshopFormatada === data;


        return corresponde;
      });

    }
    return atasFiltradas;
  }

  verDetalhesWorkshop(workshopId: number): void {
    this.router.navigate(['/workshop', workshopId]);
  }

  limparFiltros(): void {
    this.filtroForm.reset();
  }

  formatarData(data: Date): string {
    return new Date(data).toLocaleDateString('pt-BR');
  }

  obterNomesColaboradores(colaboradores: any[]): string {
    return colaboradores.map(col => col.nome).join(', ');
  }
}
