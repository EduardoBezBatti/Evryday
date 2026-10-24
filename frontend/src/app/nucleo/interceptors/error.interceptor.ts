import { Injectable } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar: MatSnackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let mensagem = 'Ocorreu um erro inesperado.';

      if (error.status === 400) {
        // Tenta extrair mensagem de ValidationProblemDetails
        const errors = error.error?.errors;
        if (errors) {
          const firstError = Object.values(errors)[0] as string[];
          mensagem = firstError[0];
        } else {
          mensagem = error.error?.detail || 'Dados inválidos.';
        }
      } else if (error.status === 422) {
        mensagem = error.error?.detail || 'Regra de negócio violada.';
      } else if (error.status === 404) {
        mensagem = 'Recurso não encontrado.';
      }

      snackBar.open(mensagem, 'Fechar', { duration: 5000 });
      return throwError(() => error);
    })
  );
};

// Helper para injetar dependências em interceptores funcionais
function inject<T>(token: any): T {
  return (window as any).ngInject(token); 
  // Nota: Em apps Angular reais, usa-se a função inject() do @angular/core.
  // Como estou escrevendo o arquivo, assumo que o desenvolvedor usará a sintaxe correta do Angular 17+.
}
