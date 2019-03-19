import { bindable, observable, computedFrom } from "aurelia-framework";
import { PersonModel } from "./PersonModel";
import { PersonSearchRequest } from "../../person-search/PersonSearchRequest";

/** A component for controlling all aspects of working with People. */
export class Person {

    /** The set of people currently being manipulated. */
    @bindable()
    public personSet: Array<PersonModel> = [];

    /** Indicates there is an active request to load data. */
    @computedFrom('activeSearch', 'activeSearch.isComplete')
    public get isLoading(): boolean {
        return this.activeSearch !== undefined && !this.activeSearch.isComplete;
    }

    /** Indicates that an active request to load data is running slowly. */
    @computedFrom('activeSearch', 'activeSearch.isSlow')
    public get isRequestSlow(): boolean {
        return this.activeSearch !== undefined && this.activeSearch.isSlow;
    }

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