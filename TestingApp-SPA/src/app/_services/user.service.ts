import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

// const httpOptions = {
//   headers: new HttpHeaders({
//     'Authorization': 'Bearer ' + localStorage.getItem('token')
//   })
// };  //This is handled by JwtModule in app module

@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
  const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  if (userParams != null) {
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
  }

  if(likesParam === 'Likers') {
    params = params.append('likers', 'true');
  }

  if(likesParam === 'Likees') {
    params = params.append('likees', 'true');
  }

  // return this.http.get<User[]>(this.baseUrl + 'users');

  return this.http.get<User[]>(this.baseUrl + 'users', {observe: 'response', params})
    .pipe(
      map(response => {
        paginatedResult.result = response.body;
        // console.log(response.headers.get('Pagination'));
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          // console.log('hello', paginatedResult.pagination);
        }
        return paginatedResult;
      })
    );

}

getUser(id): Observable<User> {
  console.log('hi:', id);
  return this.http.get<User>(this.baseUrl + 'users/' + id);
}

updateUser(id: number , user: User) {
  return this.http.put(this.baseUrl + 'users/' + id , user);
}

setMainPlantPhoto(userId: number , id: number) {
  return this.http.post(this.baseUrl + 'users/' + userId + '/plantphotos/' + id + '/setMain', {});
}

deletePlantPhoto(userId: number , id: number) {
  return this.http.delete(this.baseUrl + 'users/' + userId + '/plantphotos/' + id);
}

sendLike(id: number , recipientId: number) {
  return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientId ,{});
}

}
