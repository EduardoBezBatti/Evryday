import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { HabitosApiService } from '../../nucleo/api/habitos.service';
import { Habito, HabitoRequest, HabitoUpdateRequest } from '../../nucleo/models/habito.model';
import { HabitoFormDialogComponent } from './habito-form-dialog.component';
import { ResumoFrequenciaPipe } from '../../compartilhado/pipes/resumo-frequencia.pipe';

@Component({
  selector: 'app-habitos',
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule, 
    MatButtonModule, 
    MatIconModule, 
    MatToolbarModule, 
    MatTooltipModule,
    ResumoFrequenciaPipe
  ],
  template: `
    <mat-toolbar color="primary">
      <span>Meus Hábitos</span>
      <span class="spacer"></span>
      <button mat-raised-button color="accent" (click)="abrirFormulario()">
        <mat-icon>add</mat-icon> Novo Hábito
      </button>
    </mat-toolbar>

    <div class="container">
      <table mat-table [dataSource]="habitos" class="mat-elevation-z8">
        
        <ng-container matColumnDef="cor">
          <th mat-header-cell *matHeaderCellDef> </th>
          <td mat-cell *matCellDef="let element">
            <div class="color-dot" [style.background-color]="element.cor"></div>
          </td>
        </ng-container>

        <ng-container matColumnDef="nome">
          <th mat-header-cell *matHeaderCellDef> Nome </th>
          <td mat-cell *matCellDef="let element"> 
            <div style="display: flex; align-items: center; gap: 8px;">
              {{ element.nome }}
              <span *ngIf="element.sequenciaAtual > 0" style="font-size: 0.9rem;">
                🔥 {{ element.sequenciaAtual }}
              </span>
            </div>
          </td>
        </ng-container>

        <ng-container matColumnDef="sequencia">
          <th mat-header-cell *matHeaderCellDef> Maior Sequência </th>
          <td mat-cell *matCellDef="let element"> {{ element.maiorSequencia }} dias </td>
        </ng-container>

        <ng-container matColumnDef="frequencia">
          <th mat-header-cell *matHeaderCellDef> Frequência </th>
          <td mat-cell *matCellDef="let element"> 
            {{ element.resumoFrequencia | resumoFrequencia }} 
          </td>
        </ng-container>

        <ng-container matColumnDef="acoes">
          <th mat-header-cell *matHeaderCellDef> Ações </th>
          <td mat-cell *matCellDef="let element">
            <button mat-icon-button color="primary" (click)="editarHabito(element)" mat-tooltip="Editar">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="excluirHabito(element)" mat-tooltip="Excluir">
              <mat-icon>delete</mat-icon>
            </button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>

      <div *ngIf="habitos.length === 0" class="empty-state">
        <mat-icon>checklist</mat-icon>
        <p>Você ainda não criou nenhum hábito.</p>
      </div>
    </div>
  `,
  styles: [`
    .container { padding: 2rem; max-width: 1000px; margin: 0 auto; }
    .spacer { flex: 1 1 auto; }
    .color-dot { width: 12px; height: 12px; border-radius: 50%; display: inline-block; }
    .empty-state { display: flex; flex-direction: column; align-items: center; justify-content: center; padding: 4rem; color: #666; }
    .empty-state mat-icon { font-size: 48px; width: 48px; height: 48px; margin-bottom: 1rem; }
    table { width: 100%; }
  `]
})
export class HabitosComponent implements OnInit {
  displayedColumns: string[] = ['cor', 'nome', 'sequencia', 'frequencia', 'acoes'];
  habitos: Habito[] = [];

  constructor(
    private api: HabitosApiService, 
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.carregarHabitos();
  }

  carregarHabitos() {
    this.api.listarHabitos().subscribe(data => {
      this.habitos = data;
    });
  }

  abrirFormulario() {
    this.dialog.open(HabitoFormDialogComponent, {
      width: '400px'
    }).afterClosed().subscribe(result => {
      if (result) {
        this.salvarHabito(result);
      }
    });
  }

  editarHabito(habito: Habito) {
    this.dialog.open(HabitoFormDialogComponent, {
      width: '400px',
      data: habito
    }).afterClosed().subscribe(result => {
      if (result) {
        this.atualizarHabito(habito.id, result);
      }
    });
  }

  private salvarHabito(dados: any) {
    const req: HabitoRequest = {
      nome: dados.nome,
      descricao: dados.descricao,
      cor: dados.cor,
      dataInicio: this.formatDate(dados.dataInicio)
    };

    this.api.cadastrarHabito(req).subscribe(() => this.carregarHabitos());
  }

  private atualizarHabito(id: string, dados: any) {
    // Montagem simplificada da frequência para o exemplo
    const req: HabitoUpdateRequest = {
      nome: dados.nome,
      descricao: dados.descricao,
      cor: dados.cor,
      frequencia: { tipo: 0, dias: 0, vezesPorSemana: undefined } // Padrão diário para simplificar o form inicial
    };

    this.api.atualizarHabito(id, req).subscribe(() => this.carregarHabitos());
  }

  excluirHabito(habito: Habito) {
    if (confirm(`Tem certeza que deseja excluir "${habito.nome}"?`)) {
      this.api.excluirHabito(habito.id).subscribe(() => this.carregarHabitos());
    }
  }

  private formatDate(date: any): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }
}
