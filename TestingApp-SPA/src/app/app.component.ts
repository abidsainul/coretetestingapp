import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'TestingApp-SPA';
  jwtHelper = new JwtHelperService;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    console.log('hello');
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
    console.log('User:', user);
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = user;
      this.authService.changeMemberPlantPhotoUrl(user.photoUrl);
    }
  }
}