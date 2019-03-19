import { PersonModel } from "../pages/person/PersonModel";

/** A request which handles the search for people. */
export class PersonSearchRequest {

    /** A threshold which the request must complete within
      * in order to avoid being considered slow.
      * todo: this should come from configuration
      */
    private slowThreshold: number = 300;

    /** The http request being processed by the browser. */
    public request: Promise<{ personSet: Array<PersonModel> }>;

    /** Cancels the request. */
    public cancel: () => void;

    /** Indicates that the request has finished. */
    public isComplete: boolean = false;

    /** Indicates that the request has taken longer than the slow threshold. */
    public isSlow: boolean = false;

    /** Creates a new request for person information . */
    constructor(
        request: Promise<{ personSet: Array<PersonModel> }>
        , cancel: () => void
    ) {
        this.cancel = cancel;
        this.request =
            request
            // add a hook to the http request to mark this
            // wrapper as complete when the request has resolved.
            .then((response) => {
                this.isComplete = true;
                return Promise.resolve(response);
            })
            // add a hook to the http request to mark this
            // wrapper as complete when the request has failed.
            .catch((error) => {
                // we don't log the error out here
                // because the consumer of the PersonSearchRequest
                // is likely to add its own error handling.
                this.isComplete = true;
                return Promise.reject(error);
            })
        ;

        // create a promise which will monitor the speed of the request.
        new Promise((resolve, reject) => {
            // wait until the slow threshold has passed.
            setTimeout(() => { resolve(); }, this.slowThreshold);
        })
        .then(() => {
            // set the slow flag if the request has not yet loaded
            if (!this.isComplete) {
                this.isSlow = true;
            }
        })
        .catch((error) => { console.log(error); });
    }
}
