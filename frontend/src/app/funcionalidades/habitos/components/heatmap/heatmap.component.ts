import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface HeatmapData {
  dataInicio: string;
  dataFim: string;
  marcacoes: string[];
}

@Component({
  selector: 'app-heatmap',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './heatmap.component.html',
  styleUrls: ['./heatmap.component.css']
})
export class HeatmapComponent implements OnInit {
  @Input() data!: HeatmapData;
  @Input() corHabito: string = '#E53935';

  days: { date: Date, marked: boolean, isToday: boolean }[] = [];

  ngOnInit(): void {
    console.log('[HeatmapComponent] ngOnInit - Data recebida:', this.data);
    if (this.data) {
      this.generateCalendar();
    } else {
      console.warn('[HeatmapComponent] ngOnInit - Nenhuma data fornecida ao componente.');
    }
  }

  private generateCalendar(): void {
    console.log('[HeatmapComponent] Iniciando generateCalendar...');
    const start = new Date(this.data.dataInicio + 'T00:00:00');
    const end = new Date(this.data.dataFim + 'T00:00:00');
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    console.log(`[HeatmapComponent] Intervalo: ${this.data.dataInicio} até ${this.data.dataFim}`);

    const daysList = [];
    let current = new Date(start);

    while (current <= end) {
      // Formatação manual para evitar problemas de timezone do toISOString()
      const year = current.getFullYear();
      const month = String(current.getMonth() + 1).padStart(2, '0');
      const day = String(current.getDate()).padStart(2, '0');
      const dateStr = `${year}-${month}-${day}`;

      daysList.push({
        date: new Date(current),
        marked: this.data.marcacoes.includes(dateStr),
        isToday: current.getTime() === today.getTime()
      });
      current.setDate(current.getDate() + 1);
    }

    console.log(`[HeatmapComponent] Grade gerada com ${daysList.length} dias.`);
    this.days = daysList;
  }
}
