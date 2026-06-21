import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  template: `
    <div class="row">
      <div class="col-12">
        <h1 class="display-4">Bienvenido al Dashboard</h1>
        <p class="lead">Esta es el área privada de tu aplicación.</p>
        <hr>
        <div class="card p-4">
          <h3>Estadísticas rápidas</h3>
          <p>Aquí puedes integrar tus gráficos o resúmenes de eventos.</p>
        </div>
      </div>
    </div>
  `
})
export class DashboardComponent {}