﻿module Client.Styles

open Zanaptak.TypedCssClasses

// Font-Awesome classes
type FA = CssClasses<"https://use.fontawesome.com/releases/v5.8.1/css/all.css", Naming.PascalCase>

// Bulma classes
type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.9.1/css/bulma.css", Naming.PascalCase>

// Custom classes
type Style = CssClasses<"public/styles.css", Naming.PascalCase, resolutionFolder=__SOURCE_DIRECTORY__>
