Content in this directory is from the plugin Aurelia Tags Input.
Github: https://github.com/TGMorais/aurelia-tags-input
The author's installation intructions have not been updated so
content was initially imported into this project for usage.

This is my first time working with tag inputs or a tag input library.

Upon further prototyping modifications needed to be made to the library.
  I extended it so that clicking anywhere in the input field would give
  focus to the new tag, where previously the click only registered when
  in the leftmost 10 pixels.
  
  I modified the binding syntax to return an object of
  { value: <payload> } for the event handler, as otherwise I was
  receiving undefined in my onTagUpdate handler, and had resolved a
  similar problem the same way in the search component.

  I added additional triggers to commit a tag. Initially the library
  required that a tag be committed with the return keypress. I added
  support for the tab key, the comma, and double space. Adding a button
  would also be fairly simple to implement in a similar way.

