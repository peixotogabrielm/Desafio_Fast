import { Workshop } from './workshop.model';
import { Colaborador } from './colaborador.model';

export interface Ata {
  id: number;
  workshop: Workshop;
  colaboradores: Colaborador[];
}
