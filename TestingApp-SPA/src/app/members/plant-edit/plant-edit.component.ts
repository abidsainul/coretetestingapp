import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-plant-edit',
  templateUrl: './plant-edit.component.html',
  styleUrls: ['./plant-edit.component.css']
})
export class PlantEditComponent implements OnInit {

  constructor(private route: ActivatedRoute, private alertify: AlertifyService
    , private userService: UserService, private authService: AuthService) {}

  user: User;
  photoUrl: string;

  @ViewChild('editForm') editForm: NgForm;

  // Browser window close event
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
      //console.log("user ##");
      //console.log(this.user.plantPhotos);
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('User updated successfully');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }

  // Update Main Photo When Changes are emitting from photo editor child component
  updateMainPlantPhoto(photoUrl) {
    this.user.photoUrl = photoUrl ;
  }

}
