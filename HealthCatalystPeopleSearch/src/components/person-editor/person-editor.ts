import { bindable, autoinject } from 'aurelia-framework';
import { PersonModel, InterestModel } from '../pages/person/PersonModel';
import { HttpClient, json } from 'aurelia-fetch-client';

/** An editor for a person. */
@autoinject()
export class PersonEditor {

    /** The name of the current person. We allow this value
      * to be provided from outside so that we can improve
      * the user experience by pre-filling the first and
      * last name fields.
      */
    @bindable()
    public personName: string = "";

    /** The model of the person being edited. */
    @bindable()
    public person: PersonModel | undefined;

    /** Indicates the person being edited is being saved. */
    @bindable()
    public isSaving: boolean = false;

    /** A tag collection for the tag-input libary to bind to.
      * Unfortunately the library has some bugs and so this
      * property does not serve a purpose other than to prevent
      * compilation errors.
      */
    @bindable()
    public tags: any = [];

    /** The method invoked by the tag-input library when a new tag has been
      * added to the set. This allows us to sync up the personModel interests.
      */
    public interestSetChanged(next: Array<string>, previous: Array<string>) {
        if (this.person === undefined) { return; }
        let personId = this.person.id;

        this.person.interestSet =
            next
            // don't add whitespace as an interest
            .filter((interest: string) => interest.trim().length)
            // transform the string to an InterestModel which
            // can be committed back to the API.
            .map((interest: string) => {

                let result = {
                    summary: interest
                    , personId: personId
                    , id: newGuid()
                } as InterestModel;

                return result;
            })
        ;
    }

    /** A set of errors which need to be presented to the user. */
    public errors: Array<string> = [];

    /** Shows the editor. */
    public showEditor(): void {
        // lets try to improve the life of the user by prefilling
        // the name input with their search criteria. this will
        // need to be revisited if we need to support names with
        // more than a single space, but this MVP is very quick.

        let nameParts = this.personName.split(" ");

        // initialize a new person
        let person = {
            firstName: nameParts.length !== 0
                ? nameParts[0]
                : ""
            , lastName: nameParts.length === 2
                ? nameParts[1]
                : ""
            , age: 0
            , address: ""
            , PhotoUrl: ""
            , id: newGuid()
            , interestSet: []
        };

        this.person = person;
        this.personName = "";
    }

    /** Hides the editor. */
    public hideEditor(): void {
        if (this.person === undefined) { return; }

        // lets try to improve the life of our user by
        // filling in the search again when they haven't
        // put in new criteria. This is something the
        // business should probably make a decision on.
        if (this.personName === "") {
            let personName = this.person.firstName;

            if (this.person.lastName !== "") {
                personName += " " + this.person.lastName;
            }
            this.personName = personName;
        }

        // reset the editor state.
        this.errors = [];
        this.person = undefined;
    }

    /** Handles the Add button click event to add the new user. */
    public onAddClick(): void {
        if (this.person === undefined || this.isSaving) { return; }

        this.isSaving = true;

        let personName = this.person.firstName;

        if (this.person.lastName !== "") {
            personName += " " + this.person.lastName;
        }

        // post the request to the server to add the new user
        this.httpClient
            .fetch('api/Person/', {
                method: 'post'
                , body: json(this.person)
            })
            .then(result => {

                let isGood = result.status === 200;
                if (isGood) {
                    // handle the case where the server successfully added the new user
                    this.errors = [];
                    this.personName = personName;
                    this.person = undefined;
                }

                let isRejected = result.status === 400 || result.status === 409;
                if (isRejected) {
                    // handle the case where the server could not validate our input
                    (result.json() as Promise<{ validationErrors: Array<string> }>)
                        .then((validationResult) => {
                            this.errors = validationResult.validationErrors;
                        })
                        .catch((error) => {
                            // handle the case where something unexpected went wrong such as a network error.
                            this.errors = ["Your request could not be processed. Please try again."]
                        })
                    ;
                }

                // handle the case where something unexpected went wrong such as a network error.
                if (!isGood && !isRejected) {
                    this.errors = ["Your request could not be processed. Please try again."]
                }

                this.isSaving = false;
            })
            // handle the case where something unexpected went wrong such as a network error.
            .catch((error) => {
                this.errors = ["Your request could not be processed. Please try again."]
                this.isSaving = false;
            })
        ;
    }

    /** Constructs a new instance of the Editor, requiring the HttpClient as a dependency. */
    public constructor(private httpClient: HttpClient) { }
}

/** Creates a reasonably random aproximation of a Guid for a Proof of concept. */
export function newGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}