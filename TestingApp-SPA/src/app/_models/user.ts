import { Photo } from './photo';

export interface User {
    id: number  ;
    username: string ;
    knownAs: string ;
    age: number;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    plantPhotos?: Photo[];
}
