import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { App } from './app';

describe('App', () => {
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTesting.verify();
  });

  it('deve criar a aplicação', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();

    const req = httpTesting.expectOne('/api/saude');
    req.flush({ status: 'API ok' });
  });

  it('deve renderizar o título Evryday e o status da API', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    const req = httpTesting.expectOne('/api/saude');
    req.flush({ status: 'API ok' });

    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Evryday');
    expect(compiled.querySelector('p')?.textContent).toContain('API ok');
  });
});
