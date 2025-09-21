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
        console.log('✅ Atas carregadas do mock:', atas);
        console.log('📅 Datas dos workshops:', atas.map(a => ({
          workshop: a.workshop.nome,
          data: a.workshop.dataRealizacao,
          dataFormatada: new Date(a.workshop.dataRealizacao).toISOString().split('T')[0]
        })));
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
    console.log('🔍 Iniciando filtro com parâmetros:', { colaborador, workshop, data });
    let atasFiltradas = [...this.todasAtas];
    console.log('📋 Total de atas antes do filtro:', atasFiltradas.length);

    if (colaborador) {
      atasFiltradas = atasFiltradas.filter(ata =>
        ata.colaboradores.some(col =>
          col.nome.toLowerCase().includes(colaborador.toLowerCase())
        )
      );
      console.log('👥 Atas após filtro por colaborador:', atasFiltradas.length);
    }

    if (workshop) {
      atasFiltradas = atasFiltradas.filter(ata =>
        ata.workshop.nome.toLowerCase().includes(workshop.toLowerCase())
      );
      console.log('🏢 Atas após filtro por workshop:', atasFiltradas.length);
    }

    if (data && data.trim() !== '') {
      console.log('� Filtrando por data:', data);
      const dataOriginal = atasFiltradas.length;

      atasFiltradas = atasFiltradas.filter(ata => {
        const dataWorkshop = new Date(ata.workshop.dataRealizacao);
        const ano = dataWorkshop.getFullYear();
        const mes = String(dataWorkshop.getMonth() + 1).padStart(2, '0');
        const dia = String(dataWorkshop.getDate()).padStart(2, '0');
        const dataWorkshopFormatada = `${ano}-${mes}-${dia}`;

        console.log(`� Comparando: Workshop "${ata.workshop.nome}" - ${dataWorkshopFormatada} === ${data}`);

        const corresponde = dataWorkshopFormatada === data;
        if (corresponde) {
          console.log(`✅ Match encontrado para: ${ata.workshop.nome}`);
        }

        return corresponde;
      });

      console.log(`� Atas após filtro por data: ${atasFiltradas.length} (era ${dataOriginal})`);
    }

    console.log('🎯 Resultado final do filtro:', atasFiltradas);
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
