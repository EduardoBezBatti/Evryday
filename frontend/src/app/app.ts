import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  imports: [RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnInit {
  protected readonly title = signal('Evryday');
  protected readonly saudeApi = signal<string>('Carregando...');
  private readonly http = inject(HttpClient);

  ngOnInit(): void {
    this.http.get<{ status: string }>('/api/saude').subscribe({
      next: (res) => this.saudeApi.set(res.status),
      error: () => this.saudeApi.set('Erro ao conectar na API')
    });
  }
}
