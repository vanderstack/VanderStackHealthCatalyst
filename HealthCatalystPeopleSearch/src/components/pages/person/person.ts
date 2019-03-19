import { bindable, observable } from "aurelia-framework";
import { PersonModel } from "./PersonModel";
import { PersonSearchRequest } from "../../person-search/PersonSearchRequest";

/** A component for controlling all aspects of working with People. */
export class Person {

    /** The set of people currently being manipulated. */
    @bindable()
    public personSet: Array<PersonModel> = [];

    /** The currently running person search. */
    @bindable()
    @observable()
    public activeSearch: PersonSearchRequest | undefined;

    /** Handles a new search becoming active by attaching result and error handlers. */
    public activeSearchChanged(next: PersonSearchRequest, previous: PersonSearchRequest) {
        next
            .request
            .then((response) => {
                this.personSet = response.personSet;
            })
            .catch((error) => {
                console.log(error);
            })
        ;
    }

    /** The currently manipulated persons name. */
    @bindable()
    public personName: string = "";

    /** The method invoked by the search module when the
      * search changes so that we can stay in sync.
      */
    public onSearch(newSearch: PersonSearchRequest) {
        this.activeSearch = newSearch;
    }
}