import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HeatmapComponent } from './heatmap.component';
import { CommonModule } from '@angular/common';

describe('HeatmapComponent', () => {
  let component: HeatmapComponent;
  let fixture: ComponentFixture<HeatmapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonModule, HeatmapComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(HeatmapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  it('deve gerar a quantidade correta de dias entre dataInicio e dataFim', () => {
    // Arrange
    component.data = {
      dataInicio: '2026-10-01',
      dataFim: '2026-10-10',
      marcacoes: []
    };

    // Act
    component.ngOnInit();

    // Assert: de 01 a 10 são 10 dias
    expect(component.days.length).toBe(10);
  });

  it('deve marcar corretamente os dias que estão na lista de marcações', () => {
    // Arrange
    component.data = {
      dataInicio: '2026-10-01',
      dataFim: '2026-10-10',
      marcacoes: ['2026-10-01', '2026-10-05', '2026-10-10']
    };

    // Act
    component.ngOnInit();

    // Assert
    expect(component.days[0].marked).toBe(true);   // 01
    expect(component.days[4].marked).toBe(true);   // 05
    expect(component.days[9].marked).toBe(true);   // 10
    expect(component.days[1].marked).toBe(false);  // 02
  });

  it('deve identificar corretamente o dia de hoje', () => {
    // Arrange
    const hojeStr = new Date().toISOString().split('T')[0];
    component.data = {
      dataInicio: '2026-10-01',
      dataFim: hojeStr,
      marcacoes: []
    };

    // Act
    component.ngOnInit();

    // Assert: O último dia da lista deve ser hoje
    const lastDay = component.days[component.days.length - 1];
    expect(lastDay.isToday).toBe(true);
  });
});
