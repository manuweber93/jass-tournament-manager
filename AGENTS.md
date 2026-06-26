# Agent Instructions

- Please communicate in German, but write all code—including variable names, method names, comments, and similar elements—in English.
- Preserve existing line endings when editing files.
- Do not rewrite unrelated files.
- Keep constructor happy-path tests focused on explicitly provided values.
- Test default values separately.
- In tests, assert against explicitly arranged values or created domain objects instead of duplicating hidden test-data defaults as literals.
- Keep methods readable and maintainable; when a method starts mixing multiple responsibilities or becomes hard to scan, extract small, intention-revealing helper methods.
- Avoid duplicating shared logic; extract reusable helpers or abstractions while keeping the caller's intent clear.
- Do not run app builds unless the user explicitly asks for a build. If the user explicitly asks for a build, let it run for at least 60 seconds when possible.
