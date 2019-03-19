import { bindable } from 'aurelia-framework';
import { PersonModel, InterestModel } from '../pages/person/PersonModel';

/** A grid which shows an overview of many people. */
export class PersonGrid {

    /** Indicates that the grid is loading information. */
    @bindable()
    public isLoading: boolean = false;

    /** Indicates that the request for information is running slow. */
    @bindable()
    public isRequestSlow: boolean = false;

    /** The set of people which will be rendered into the grid. */
    @bindable()
    public personSet: Array<PersonModel> = [];

    public stockPhotoUrl: string = '/blank-profile.png';

    /** Formats the provided set of InterestModels to a comma separated list. */
    public formatInterests(interestSet: Array<InterestModel>): string {
        return interestSet
            .map((interest) =>
                interest.summary
            )
            .join(", ")
        ;
    }
}