import { bindable } from "aurelia-framework";

/** A component for controlling all aspects of working with People. */
export class Person {

    /** The currently manipulated persons name. */
    @bindable()
    public personName: string = "";
}