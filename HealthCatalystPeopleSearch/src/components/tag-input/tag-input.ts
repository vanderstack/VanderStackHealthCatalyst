import { bindable } from "aurelia-framework";

export class TagInput {

    @bindable()
    tags: Array<any>;

    @bindable()
    onChanged: any;

    inputElement: any;

    constructor() {
        this.tags = [];

        this.onClick = this.onClick.bind(this);

        this.onTagClick = this.onTagClick.bind(this);
        this.onTagBlur = this.onTagBlur.bind(this);
        this.onTagKey = this.onTagKey.bind(this);
    }

    tagsChanged(value: any) {
        if (value instanceof Array) {
            this.tags = value.map(value => { return { value } });
            this.addNewTag();
        }
    }

    onClick(e: any) {
        if (this.tags.length && this.inputElement === e.target) {
            this.tags[this.tags.length - 1].focus = true;
        }
    }

    updateTags() {
        if (typeof this.onChanged === 'function') {
            let next = this.tags.map((t: { value: any; }) => { return t.value });
            this.onChanged({ value: next});
        }
    }

    addNewTag() {
        if (!this.tags.find((x: { editing: any; }) => x.editing)) {
            this.addTag('', true, true);
        }
    }

    addTag(value: any, editing = false, focus = false) {
        this.tags.push({
            value,
            editing,
            focus,
        });

        if (value) {
            this.updateTags();
        }
    }

    removeTag(tag: any) {
        let idx = this.tags.indexOf(tag);
        if (idx > -1) {
            this.tags.splice(idx, 1);
            this.updateTags();
        }
    }

    editTag(tag: any, edit: any) {
        tag.editing = edit;
        if (!edit) {
            this.updateTags();
        }
    }

    onTagClick(tag: any, action: any) {
        if (action === 'delete') {
            this.removeTag(tag);
            return;
        }

        this.editTag(tag, true);
    }

    onTagBlur(tag: any, e: any) {
        let emptyTag = (!tag.value || !tag.value.length);
        let lastTag = (this.tags.indexOf(tag) === this.tags.length - 1);

        if (!emptyTag) {
            this.editTag(tag, false);

            if (lastTag) {
                this.addNewTag();
            }
        }

        if (!lastTag && emptyTag) {
            this.removeTag(tag);
        }
    }

    onTagKey(tag: any, e: any) {
        let key = e.which || e.keyCode;
        let finishTag =
            key === 13 // enter
            || key === 9 // tab
            || key === 44 // comma
            || (
                // two consecutive spaces
                e.key === ' '
                && !tag.value.slice(-1).trim().length
            )
        ;

        if (!finishTag) {
            return true;
        }

        // remove whitespace
        tag.value = tag.value.trim();

        let isDuplicate =
            undefined !== this.tags.find((candidateTag) =>
                tag !== candidateTag
                && tag.value === candidateTag.value
            )
        ;

        // do not duplicate a tag
        if (isDuplicate) {
            // another tag already has this input
            // remove this tag.
            // this.removeTag(tag);
            tag.value = "";
        } else {
            // mark this tag as no longer being edited
            this.editTag(tag, false);

            // create a new tag which will take the next input
            this.addNewTag();
        }
            
        return false;
    }
}