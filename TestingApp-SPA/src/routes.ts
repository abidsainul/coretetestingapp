import { Routes } from '@angular/router';
import { HomeComponent } from './app/home/home.component';
import { PlantListComponent } from './app/members/plant-list/plant-list.component';
import { PlantDetailComponent } from './app/members/plant-detail/plant-detail.component';

import { ListsComponent } from './app/lists/lists.component';
import { MessagesComponent } from './app/messages/messages.component';
import { AuthGuard } from './app/_guards/auth.guard';
import { PlantDetailResolver } from './app/_resolvers/plant-detail-resolver';
import { PlantListResolver } from './app/_resolvers/plant-list-resolver';

export const appRoutes: Routes = [

    {path : '' , component : HomeComponent },
    {
        path: '',
        runGuardsAndResolvers : 'always',
        canActivate : [AuthGuard],
        children: [
            {path : 'plants' , component : PlantListComponent,
                    resolve: { users : PlantListResolver }},
            {path : 'plants/:id' , component : PlantDetailComponent,
                resolve: {user: PlantDetailResolver}},
            {path : 'messages' , component : MessagesComponent},
            {path : 'lists' , component : ListsComponent}
        ]
    },
    {path : '**' , redirectTo : '' , pathMatch : 'full'},

];
