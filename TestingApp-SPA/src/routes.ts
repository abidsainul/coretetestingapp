import { Routes } from '@angular/router';
import { HomeComponent } from './app/home/home.component';
import { PlantListComponent } from './app/plant-list/plant-list.component';
import { ListsComponent } from './app/lists/lists.component';
import { MessagesComponent } from './app/messages/messages.component';
import { AuthGuard } from './app/_guards/auth.guard';

export const appRoutes: Routes = [

    {path : '' , component : HomeComponent },
    {
        path: '',
        runGuardsAndResolvers : 'always',
        canActivate : [AuthGuard],
        children: [
            {path : 'plants' , component : PlantListComponent},
            {path : 'messages' , component : MessagesComponent},
            {path : 'lists' , component : ListsComponent}
        ]
    },
    {path : '**' , redirectTo : '' , pathMatch : 'full'},

];
