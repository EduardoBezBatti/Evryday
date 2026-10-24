import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { HabitosApiService } from '../../nucleo/api/habitos.service';
import { HojeResponse, HabitoHoje } from '../../nucleo/models/habito.model';

@Component({
  selector: 'app-hoje',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatToolbarModule
  ],
  template: `
    <mat-toolbar color="primary">
      <span>Hoje</span>
      <span class="spacer"></span>
      <button mat-button routerLink="/habitos">Meus Hábitos</button>
    </mat-toolbar>

    <div class="container">
      <header class="header">
        <h1>{{ dataHoje | date:'fullDate' }}</h1>
        <div class="progress-container">
          <div class="progress-label">
            <span>Progresso do Dia</span>
            <span>{{ (dadosHoje?.progressoSemanal || 0) | percent }}</span>
          </div>
          <mat-progress-bar mode="determinate" [value]="dadosHoje?.progressoSemanal"></mat-progress-bar>
        </div>
      </header>

      <section *ngIf="dadosHoje?.paraHoje?.length">
        <h3>Para Hoje</h3>
        <mat-list>
          <mat-list-item *ngFor="let habito of dadosHoje?.paraHoje">
            <mat-icon matListItemIcon [style.color]="habito.cor">fiber_manual_record</mat-icon>
            <div matListItemTitle class="habit-info">
              {{ habito.nome }}
              <span class="streak-badge" *ngIf="habito.sequenciaAtual > 0">
                🔥 {{ habito.sequenciaAtual }}
              </span>
            </div>
            <button mat-icon-button [color]="habito.marcado ? 'primary' : ''" (click)="toggleMarcacao(habito)">
              <mat-icon>{{ habito.marcado ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
            </button>
          </mat-list-item>
        </mat-list>
      </section>

      <section *ngIf="dadosHoje?.extras?.length">
        <h3>Extras (Não previstos)</h3>
        <mat-list>
          <mat-list-item *ngFor="let habito of dadosHoje?.extras">
            <mat-icon matListItemIcon [style.color]="habito.cor">fiber_manual_record</mat-icon>
            <div matListItemTitle class="habit-info">
              {{ habito.nome }}
              <span class="streak-badge" *ngIf="habito.sequenciaAtual > 0">
                🔥 {{ habito.sequenciaAtual }}
              </span>
            </div>
            <button mat-icon-button color="primary" (click)="toggleMarcacao(habito)">
              <mat-icon>check_circle</mat-icon>
            </button>
          </mat-list-item>
        </mat-list>
      </section>

      <div *ngIf="!dadosHoje?.paraHoje?.length && !dadosHoje?.extras?.length" class="empty-state">
        <mat-icon>celebration</mat-icon>
        <p>Nada planejado para hoje. Aproveite!</p>
      </div>
    </div>
  `,
  styles: [`
    .container { padding: 2rem; max-width: 600px; margin: 0 auto; }
    .spacer { flex: 1 1 auto; }
    .header { margin-bottom: 2rem; text-align: center; }
    .progress-container { margin-top: 1rem; }
    .progress-label { display: flex; justify-content: space-between; margin-bottom: 0.5rem; font-weight: 500; }
    section { margin-bottom: 2rem; }
    h3 { color: #666; font-size: 1.2rem; margin-bottom: 1rem; border-bottom: 1px solid #eee; padding-bottom: 0.5rem; }
    .habit-info { display: flex; align-items: center; gap: 8px; }
    .streak-badge { font-size: 0.85rem; font-weight: bold; color: #ff5722; background: #fff3e0; padding: 2px 6px; border-radius: 10px; }
    .empty-state { display: flex; flex-direction: column; align-items: center; justify-content: center; padding: 4rem; color: #666; text-align: center; }
    .empty-state mat-icon { font-size: 48px; width: 48px; height: 48px; margin-bottom: 1rem; }
  `]
})
export class HojeComponent implements OnInit {
  dataHoje = new Date();
  dadosHoje: HojeResponse | null = null;

  constructor(private api: HabitosApiService) {}

  ngOnInit() {
    this.carregarHoje();
  }

  carregarHoje() {
    this.api.obterHoje().subscribe(data => {
      this.dadosHoje = data;
    });
  }

  toggleMarcacao(habito: any) {
    const dataStr = this.formatDate(this.dataHoje);
    if (habito.marcado) {
      this.api.desmarcarHabito(habito.id, dataStr).subscribe(() => this.carregarHoje());
    } else {
      this.api.marcarHabito(habito.id, dataStr).subscribe(() => this.carregarHoje());
    }
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }
}
