import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'resumoFrequencia',
  standalone: true
})
export class ResumoFrequenciaPipe implements PipeTransform {
  private readonly diasNomes: Record<number, string> = {
    0: 'Dom', 1: 'Seg', 2: 'Ter', 3: 'Qua', 4: 'Qui', 5: 'Sex', 6: 'Sáb'
  };

  transform(valor: any): string {
    if (!valor) return '';

    const { tipo, dias, vezesPorSemana } = valor;

    // 0: Diária
    if (tipo === 0) return 'Diariamente';

    // 1: Dias da Semana
    if (tipo === 1) {
      const selecionados: string[] = [];
      for (let i = 0; i < 7; i++) {
        if ((dias & (1 << i)) !== 0) {
          selecionados.push(this.diasNomes[i]);
        }
      }
      return selecionados.length > 0 
        ? selecionados.join(', ') 
        : 'Nenhum dia selecionado';
    }

    // 2: Vezes por semana
    if (tipo === 2) {
      return `${vezesPorSemana}x por semana`;
    }

    return 'Frequência desconhecida';
  }
}
