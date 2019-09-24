import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { PlantEditComponent } from '../members/plant-edit/plant-edit.component';
@Injectable()

export class PreventUnsavedChanges implements CanDeactivate<PlantEditComponent> {
    canDeactivate(component: PlantEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue ? All unsaved changes will be lost!');
        }
        return true;
    }
}