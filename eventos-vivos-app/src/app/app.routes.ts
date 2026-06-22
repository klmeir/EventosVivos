import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';

export const routes: Routes = [
    { path: '', redirectTo: '/events', pathMatch: 'full' },
    { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
    {
        path: '',
        component: MainLayoutComponent,
        children: [            
            { path: 'events', loadComponent: () => import('./features/events/event-list/event-list.component').then(m => m.EventListComponent) },
            { path: 'events/create', loadComponent: () => import('./features/events/event-form/event-form.component').then(m => m.EventFormComponent) },
            { path: 'events/details/:id', loadComponent: () => import('./features/events/event-details/event-details.component').then(m => m.EventDetailsComponent) },
            { path: 'events/report/:id', loadComponent: () => import('./features/events/event-report/event-report.component').then(m => m.EventReportComponent) },
        ]
  }
];
