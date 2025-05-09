# mnemosyne

Putting together a web app that is a little more complicated than a todo list to give myself a framework for trying new things.

Mnemosyne is intended to be a note-taking application with support for images that will eventually have web and mobile front ends. Inspired partially by all of the notes I have been taking while playing Blue Prince.

Current stack is:
- PostgreSQL
- .NET9
- EF Core DB First
- Keycloak
- Vue (+ Vite)
- Tailwind

Todos:
 - [x] Switch notebooks to use UUIDs
 - [ ] Switch the notebook saving to use the SignalR hub
 - [ ] Add pushing notebook changes up to clients if they are not the originating client
 - [x] Fix up the UI to something usable for switching notebooks
 - [ ] Clean up the tailwind classes via @apply or switch away from tailwind (sorry, all the classes make the markup ugly)
