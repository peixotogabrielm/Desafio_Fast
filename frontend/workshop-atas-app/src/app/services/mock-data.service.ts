import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Ata, Workshop, Colaborador } from '../models';

@Injectable({
  providedIn: 'root'
})
export class MockDataService {
  private colaboradores: Colaborador[] = [
    { id: 1, nome: 'João Silva' },
    { id: 2, nome: 'Maria Santos' },
    { id: 3, nome: 'Carlos Oliveira' },
    { id: 4, nome: 'Ana Costa' },
    { id: 5, nome: 'Pedro Ferreira' },
    { id: 6, nome: 'Lucia Rodrigues' },
    { id: 7, nome: 'Roberto Lima' },
    { id: 8, nome: 'Fernanda Souza' },
    { id: 9, nome: 'Gabriel Mendes' },
    { id: 10, nome: 'Patricia Alves' }
  ];

  private workshops: Workshop[] = [
    {
      id: 1,
      nome: 'Matemática aplicada',
      dataRealizacao: new Date('2024-01-15'),
      descricao: 'Workshop introdutório sobre Matemática aplicada, cobrindo conceitos básicos e aplicações práticas'
    },
    {
      id: 2,
      nome: 'Geometria Analítica',
      dataRealizacao: new Date('2024-02-20'),
      descricao: 'Conceitos avançados de Geometria Analítica incluindo retas, planos e superfícies'
    },
    {
      id: 3,
      nome: 'Aulas sobre React',
      dataRealizacao: new Date('2024-03-10'),
      descricao: 'Aulas práticas sobre React, abordando componentes funcionais, hooks e roteamento com React Router'
    },
    {
      id: 4,
      nome: 'AngularJs',
      dataRealizacao: new Date('2024-04-05'),
      descricao: 'AngularJs conceitos e práticas para desenvolvimento de aplicações web'
    },
    {
      id: 5,
      nome: 'Docker para Desenvolvedores',
      dataRealizacao: new Date('2024-05-12'),
      descricao: 'Containerização de aplicações web com Docker e Docker Compose'
    }
  ];

  private atas: Ata[] = [
    {
      id: 1,
      workshop: this.workshops[0],
      colaboradores: [
        this.colaboradores[0],
        this.colaboradores[1],
        this.colaboradores[2],
        this.colaboradores[3]
      ]
    },
    {
      id: 2,
      workshop: this.workshops[1],
      colaboradores: [
        this.colaboradores[1],
        this.colaboradores[2],
        this.colaboradores[4],
        this.colaboradores[5],
        this.colaboradores[6]
      ]
    },
    {
      id: 3,
      workshop: this.workshops[2],
      colaboradores: [
        this.colaboradores[0],
        this.colaboradores[3],
        this.colaboradores[6],
        this.colaboradores[7],
        this.colaboradores[8]
      ]
    },
    {
      id: 4,
      workshop: this.workshops[3],
      colaboradores: [
        this.colaboradores[2],
        this.colaboradores[5],
        this.colaboradores[7],
        this.colaboradores[9]
      ]
    },
    {
      id: 5,
      workshop: this.workshops[4],
      colaboradores: [
        this.colaboradores[0],
        this.colaboradores[4],
        this.colaboradores[6],
        this.colaboradores[8],
        this.colaboradores[9]
      ]
    }
  ];

  constructor() {}

  obterAtas(): Observable<Ata[]> {
    return of(this.atas);
  }

  obterAtasPorWorkshopNome(workshopNome: string): Observable<Ata[]> {
    const atasFiltradas = this.atas.filter(ata =>
      ata.workshop.nome.toLowerCase().includes(workshopNome.toLowerCase())
    );
    return of(atasFiltradas);
  }

  obterAtasPorColaboradorNome(colaboradorNome: string): Observable<Ata[]> {
    const atasFiltradas = this.atas.filter(ata =>
      ata.colaboradores.some(colaborador =>
        colaborador.nome.toLowerCase().includes(colaboradorNome.toLowerCase())
      )
    );
    return of(atasFiltradas);
  }

  obterAtasPorData(data: Date): Observable<Ata[]> {
    const atasFiltradas = this.atas.filter(ata => {
      const dataWorkshop = new Date(ata.workshop.dataRealizacao);
      const dataFiltro = new Date(data);
      return dataWorkshop.toDateString() === dataFiltro.toDateString();
    });
    return of(atasFiltradas);
  }

  obterWorkshopPorId(id: number): Observable<Workshop | undefined> {
    const workshop = this.workshops.find(w => w.id === id);
    return of(workshop);
  }

  obterColaboradores(): Observable<Colaborador[]> {
    return of(this.colaboradores);
  }

  obterWorkshops(): Observable<Workshop[]> {
    return of(this.workshops);
  }
}
