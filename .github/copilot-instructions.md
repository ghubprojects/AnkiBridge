\# Commit Message Custom Instructions (with Gitmoji)



\## Specification



Generate commit messages following enterprise Git standards \*\*with Gitmoji\*\*.



\---



\## Rules



Use \*\*Gitmoji\*\* format for commit messages.



The format should be:



```

<intention> \[<scope>?] <message>

```



\- `<intention>`: The emoji indicating the intention of the commit. Can be written in shortcode format (e.g., `:bug:`) or unicode (e.g., `🐛`).

\- `\[<scope>]`: \*(optional)\* A string indicating the scope of the change.

\- `<message>`: A brief explanation of the change, written in present tense, imperative mood, and should not exceed 72 characters.



\---



\## General Commit Message Rules



\- Use English for commit messages.

\- Keep the summary ≤ 72 characters.

\- \*\*Capitalize\*\* the first letter of the summary.

\- Do \*\*not\*\* end the summary with a period.

\- The intention should be one of the allowed Gitmoji emojis (see the allowed list below).

\- If a scope is used, it should be enclosed in parentheses (e.g., `(auth)`).



\---



\## Allowed Intention Emojis



Can be written in shortcode format or unicode:



| Emoji | Shortcode | Description |

|-------|-----------|-------------|

| 🎨 | `:art:` | Improve structure / format of the code. |

| ⚡️ | `:zap:` | Improve performance. |

| 🔥 | `:fire:` | Remove code or files. |

| 🐛 | `:bug:` | Fix a bug. |

| 🚑️ | `:ambulance:` | Critical hotfix. |

| ✨ | `:sparkles:` | Introduce new features. |

| 📝 | `:memo:` | Add or update documentation. |

| 🚀 | `:rocket:` | Deploy stuff. |

| 💄 | `:lipstick:` | Add or update the UI and style files. |

| 🎉 | `:tada:` | Begin a project. |

| ✅ | `:white\_check\_mark:` | Add, update, or pass tests. |

| 🔒️ | `:lock:` | Fix security or privacy issues. |

| 🔐 | `:closed\_lock\_with\_key:` | Add or update secrets. |

| 🔖 | `:bookmark:` | Release / Version tags. |

| 🚨 | `:rotating\_light:` | Fix compiler / linter warnings. |

| 🚧 | `:construction:` | Work in progress. |

| 💚 | `:green\_heart:` | Fix CI Build. |

| ⬇️ | `:arrow\_down:` | Downgrade dependencies. |

| ⬆️ | `:arrow\_up:` | Upgrade dependencies. |

| 📌 | `:pushpin:` | Pin dependencies to specific versions. |

| 👷 | `:construction\_worker:` | Add or update CI build system. |

| 📈 | `:chart\_with\_upwards\_trend:` | Add or update analytics or track code. |

| ♻️ | `:recycle:` | Refactor code. |

| ➕ | `:heavy\_plus\_sign:` | Add a dependency. |

| ➖ | `:heavy\_minus\_sign:` | Remove a dependency. |

| 🔧 | `:wrench:` | Add or update configuration files. |

| 🔨 | `:hammer:` | Add or update development scripts. |

| 🌐 | `:globe\_with\_meridians:` | Internationalization and localization. |

| ✏️ | `:pencil2:` | Fix typos. |

| 💩 | `:poop:` | Write bad code that needs to be improved. |

| ⏪️ | `:rewind:` | Revert changes. |

| 🔀 | `:twisted\_rightwards\_arrows:` | Merge branches. |

| 📦️ | `:package:` | Add or update compiled files or packages. |

| 👽️ | `:alien:` | Update code due to external API changes. |

| 🚚 | `:truck:` | Move or rename resources (e.g., files, paths, routes). |

| 📄 | `:page\_facing\_up:` | Add or update license. |

| 💥 | `:boom:` | Introduce breaking changes. |

| 🍱 | `:bento:` | Add or update assets. |

| ♿️ | `:wheelchair:` | Improve accessibility. |

| 💡 | `:bulb:` | Add or update comments in source code. |

| 🍻 | `:beers:` | Write code drunkenly. |

| 💬 | `:speech\_balloon:` | Add or update text and literals. |

| 🗃️ | `:card\_file\_box:` | Perform database related changes. |

| 🔊 | `:loud\_sound:` | Add or update logs. |

| 🔇 | `:mute:` | Remove logs. |

| 👥 | `:busts\_in\_silhouette:` | Add or update contributor(s). |

| 🚸 | `:children\_crossing:` | Improve user experience / usability. |

| 🏗️ | `:building\_construction:` | Make architectural changes. |

| 📱 | `:iphone:` | Work on responsive design. |

| 🤡 | `:clown\_face:` | Mock things. |

| 🥚 | `:egg:` | Add or update an easter egg. |

| 🙈 | `:see\_no\_evil:` | Add or update a .gitignore file. |

| 📸 | `:camera\_flash:` | Add or update snapshots. |

| ⚗️ | `:alembic:` | Perform experiments. |

| 🔍️ | `:mag:` | Improve SEO. |

| 🏷️ | `:label:` | Add or update types. |

| 🌱 | `:seedling:` | Add or update seed files. |

| 🚩 | `:triangular\_flag\_on\_post:` | Add, update, or remove feature flags. |

| 🥅 | `:goal\_net:` | Catch errors. |

| 💫 | `:dizzy:` | Add or update animations and transitions. |

| 🗑️ | `:wastebasket:` | Deprecate code that needs to be cleaned up. |

| 🛂 | `:passport\_control:` | Work on code related to authorization, roles, and permissions. |

| 🩹 | `:adhesive\_bandage:` | Simple fix for a non-critical issue. |

| 🧐 | `:monocle\_face:` | Data exploration/inspection. |

| ⚰️ | `:coffin:` | Remove dead code. |

| 🧪 | `:test\_tube:` | Add a failing test. |

| 👔 | `:necktie:` | Add or update business logic. |

| 🩺 | `:stethoscope:` | Add or update healthcheck. |

| 🧱 | `:bricks:` | Infrastructure related changes. |

| 🧑‍💻 | `:technologist:` | Improve developer experience. |

| 💸 | `:money\_with\_wings:` | Add sponsorships or money-related infrastructure. |

| 🧵 | `:thread:` | Add or update code related to multithreading or concurrency. |

| 🦺 | `:safety\_vest:` | Add or update code related to validation. |

| ✈️ | `:airplane:` | Improve offline support. |

| 🦖 | `:t-rex:` | Code that adds backwards compatibility. |



\---



\## Body Rules



\- Add a blank line between summary and body.

\- Use \*\*bullet points\*\* for each section in the body.

\- The first letter after each bullet point should be \*\*capitalized\*\*.

\- Explain \*what\* and \*why\*, not implementation details.

\- Wrap lines at 72 characters.



\---



\## Footer Rules \*(optional)\*



\- Reference issues using `Refs: #123`

\- For breaking changes add: `BREAKING CHANGE:`



\---



\## Examples



```

⚡️ Lazyload home screen images

```



```

🐛 Fix `onClick` event handler

```



```

🔖 Bump version `1.2.0`

```



```

♻️ (components): Transform classes to hooks

```



```

📈 Add analytics to the dashboard

```



```

🌐 Support Japanese language

```



```

♿️ (account): Improve modals a11y

```



\## Example Commit Message with Body



```

♻️ (domain): Refactor and expand domain model for learning app



\- Remove obsolete code

\- Improve extensibility and DDD alignment

```

