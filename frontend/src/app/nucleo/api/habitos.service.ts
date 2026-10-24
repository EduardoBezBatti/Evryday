import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Habito, HabitoRequest, HabitoUpdateRequest, HojeResponse } from '../models/habito.model';

@Injectable({
  providedIn: 'root'
})
export class HabitosApiService {
  private readonly apiUrl = '/api';

  constructor(private http: HttpClient) {}

  // T3.3
  listarHabitos(): Observable<Habito[]> {
    return this.http.get<Habito[]>(`${this.apiUrl}/habitos`);
  }

  buscarHabito(id: string): Observable<Habito> {
    return this.http.get<Habito>(`${this.apiUrl}/habitos/${id}`);
  }

  cadastrarHabito(req: HabitoRequest): Observable<Habito> {
    return this.http.post<Habito>(`${this.apiUrl}/habitos`, req);
  }

  // T3.4
  atualizarHabito(id: string, req: HabitoUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/habitos/${id}`, req);
  }

  // T3.5
  excluirHabito(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/habitos/${id}`);
  }

  // T3.6
  obterHoje(): Observable<HojeResponse> {
    return this.http.get<HojeResponse>(`${this.apiUrl}/hoje`);
  }

  // T3.7
  marcarHabito(id: string, data: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/habitos/${id}/marcacoes/${data}`, {});
  }

  desmarcarHabito(id: string, data: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/habitos/${id}/marcacoes/${data}`);
  }
}
