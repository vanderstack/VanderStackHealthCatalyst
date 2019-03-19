/** The model of a person as returned by the Api.*/
export interface PersonModel {
    id: string;
    firstName: string;
    lastName: string;
    address: string;
    age: number;
    interestSet: Array<InterestModel>;
    PhotoUrl: string;
}

/** The model of a persons interests as returned by the Api.*/
export interface InterestModel {
    id: string;
    personId: string;
    summary: string;
}