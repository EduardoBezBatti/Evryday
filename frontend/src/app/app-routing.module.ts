import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HojeComponent } from './funcionalidades/hoje/hoje.component';
import { HabitosComponent } from './funcionalidades/habitos/habitos.component';

const routes: Routes = [
  { path: '', redirectTo: 'hoje', pathMatch: 'full' },
  { path: 'hoje', component: HojeComponent },
  { path: 'habitos', component: HabitosComponent },
  { path: '**', redirectTo: 'hoje' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
