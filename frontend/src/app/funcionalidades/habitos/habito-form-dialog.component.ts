import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { HeatmapComponent } from './components/heatmap/heatmap.component';
import { HabitosApiService } from '../../nucleo/api/habitos.service';

@Component({
  selector: 'app-habito-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    HeatmapComponent
  ],
  template: `
    <h2 mat-dialog-title>{{ data?.id ? 'Editar' : 'Novo' }} Hábito</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="form-container">
        <mat-form-field appearance="outline">
          <mat-label>Nome</mat-label>
          <input matInput formControlName="nome" placeholder="Ex: Beber Água">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Descrição</mat-label>
          <textarea matInput formControlName="descricao" placeholder="Opcional..."></textarea>
        </mat-form-field>

        <div class="color-picker">
          <label>Cor do Hábito</label>
          <div class="palette">
            <div *ngFor="let color of palette"
                 (click)="selectColor(color)"
                 [class.selected]="selectedColor === color"
                 [style.background-color]="color"
                 class="color-circle">
            </div>
          </div>
        </div>

        <mat-form-field appearance="outline">
          <mat-label>Data de Início</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="dataInicio">
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
        </mat-form-field>
      </form>

      <div *ngIf="data?.id && historico" class="heatmap-section">
        <h3>Histórico de Marcações</h3>
        <app-heatmap [data]="historico" [corHabito]="selectedColor"></app-heatmap>
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancelar</button>
      <button mat-raised-button color="primary" [disabled]="form.invalid" (click)="onSave()">Salvar</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .form-container { display: flex; flex-direction: column; gap: 1rem; min-width: 300px; padding-top: 1rem; }
    mat-form-field { width: 100%; }
    .color-picker { display: flex; flex-direction: column; gap: 0.5rem; margin-bottom: 1rem; }
    .palette { display: flex; gap: 0.5rem; flex-wrap: wrap; }
    .color-circle { width: 30px; height: 30px; border-radius: 50%; cursor: pointer; border: 2px solid transparent; transition: transform 0.2s; }
    .color-circle:hover { transform: scale(1.2); }
    .color-circle.selected { border-color: #000; transform: scale(1.2); }
    .heatmap-section { margin-top: 2rem; border-top: 1px solid #eee; padding-top: 1rem; display: flex; flex-direction: column; align-items: center; }
    .heatmap-section h3 { font-size: 1rem; color: #666; margin-bottom: 1rem; font-weight: 500; }
  `]
})
export class HabitoFormDialogComponent implements OnInit {
  palette = ['#FF5252', '#FF4081', '#E040FB', '#7C4DFF', '#536DFE', '#448AFF', '#40C4FF', '#18FFFF'];
  selectedColor = '#FF5252';
  historico: any = null;

  form = new FormGroup({
    nome: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(60)] }),
    descricao: new FormControl('', { nonNullable: true, validators: [Validators.maxLength(200)] }),
    cor: new FormControl('#FF5252', { nonNullable: true, validators: [Validators.required] }),
    dataInicio: new FormControl(new Date(), { nonNullable: true, validators: [Validators.required] })
  });

  constructor(
    public dialogRef: MatDialogRef<HabitoFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private habitosService: HabitosApiService
  ) {
  }

  ngOnInit(): void {
    if (this.data) {
      this.form.patchValue({
        nome: this.data.nome,
        descricao: this.data.descricao,
        cor: this.data.cor,
        dataInicio: new Date(this.data.dataInicio)
      });
      this.selectedColor = this.data.cor;

      if (this.data.id) {
        this.carregarHistorico(this.data.id);
      }
    }
  }

  private carregarHistorico(id: string): void {
    console.log(`[HabitoFormDialog] Carregando histórico para o hábito: ${id}`);
    this.habitosService.obterHistorico(id).subscribe({
      next: (res) => {
        console.log('[HabitoFormDialog] Histórico recebido:', res);
        this.historico = res;
      },
      error: (err) => console.error('[HabitoFormDialog] Erro ao carregar histórico:', err)
    });
  }

  selectColor(color: string) {
    this.selectedColor = color;
    this.form.get('cor')?.setValue(color);
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSave() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }
}
