import { HttpClient, RequestInit } from 'aurelia-fetch-client';
import { inject, bindable, observable } from 'aurelia-framework';
import { PersonSearchRequest } from './PersonSearchRequest';
import { PersonModel } from '../pages/person/PersonModel';

/** Performs searches for People. */
@inject(HttpClient)
export class PersonSearch {
    
    /** A method passed in from outside which we invoke each time a search starts to keep
     * consumers informed about the search status.
     */
    @bindable()
    public onSearch: (next: { newSearch: PersonSearchRequest }) => void = (next) => { };
    
    /** The name currently being searched for. */
    @bindable()
    @observable()
    public personName: string = "";

    /** Handles the name changing to trigger a new search. */
    public personNameChanged(next: string, previous: string) {
        this.search(next);
    }

    /** The search request actively being processed by the browser and application. */
    @observable()
    public activeRequest: PersonSearchRequest | undefined = undefined;

    /** Handles the active request changing, including cancelling the previous request
      * and notifying the outside of the new request by invoking onSearch.
      */
    public activeRequestChanged(next: PersonSearchRequest, previous: PersonSearchRequest) {

        if (previous && !previous.isComplete) {
            previous.cancel()
        }

        this.onSearch({ newSearch: next });
    }

    /** Creates a new instance of the search. Requires the Http Client as a dependency */
    constructor(private httpClient: HttpClient) { }

    /** Initiates a new Api Search Http request. */
    private search(token: string) {
        if (token === undefined) { return; }

        token = token.trim();
        if (token === "") { return; }

        // create a new controller responsible for aborting this request.
        // we use a unique controller per request so that we could have
        // multiple instances of the search component running at the same time
        // and not have them cancel each others requests.
        let controller = new AbortController();
        let cancel = () => {
            controller.abort();
        }

        this.activeRequest =
            new PersonSearchRequest(
                this.httpClient
                    .fetch(`api/Person/Search/${token}`, {
                        signal: controller.signal
                    } as RequestInit)
                    .then(result => result.json() as Promise<{ personSet: Array<PersonModel> }>)
                    // no error handling because the consumer of the personSearchRequest
                    // is likely to implement its own error handling.
                , cancel
            )
        ;
    }
}

