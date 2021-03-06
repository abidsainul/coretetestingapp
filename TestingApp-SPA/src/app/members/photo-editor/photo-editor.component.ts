import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

import { Photo } from '../../_models/photo';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getPlantPhotoChange = new EventEmitter<string>();

  currentMain: Photo;

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/plantphotos',
      authToken : 'Bearer ' + localStorage.getItem('token'),
      isHTML5 : true,
      allowedFileType : ['image'],
      removeAfterUpload : true,
      autoUpload : false,
      maxFileSize : 10 * 1024 * 1024
    });

    // cors fix
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id : res.id,
          url : res.url,
          description: res.description,
          dateAdded: res.dateAdded,
          isMain: res.isMain
        };
        this.photos.push(photo);
        // Update compnents if first photo (main will be true)
        if (photo.isMain) {
          this.authService.changeMemberPlantPhotoUrl(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };
  }

  setMainPlantPhoto(photo: Photo){
    this.userService.setMainPlantPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
      this.currentMain = this.photos.filter(p => p.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;

      // output to parent component
      //this.getPlantPhotoChange.emit(photo.url);

        this.authService.changeMemberPlantPhotoUrl(photo.url);
        this.authService.currentUser.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePlantPhoto(id: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deletePlantPhoto(this.authService.decodedToken.nameid, id).subscribe ( () => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertify.success('Photo has been deleted')
      }, error => {
        this.alertify.error('Failed to delete the photo')
      });
    });
  }

}
