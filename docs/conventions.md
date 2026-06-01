# Testing
## Naming
The name of test methods is constructed as follows:
`<name of method>_<state / condition>_<expected result>`


The expected result is written in active tense, i.e. "IsCompleted" rather than "ShouldBeCompleted" or "ThrowsArgumentsException" rather than "ShouldThrowArgumentException".

Example: Tournament.Constructor_WithEmptyOrganizerId_ThrowsArgumentException()

## Constructor tests
Successful constructor tests verify explicitly provided values only.

Default values are tested in separate test methods.

# Application Layer

## Result pattern
Application services return `Result<T>` instead of throwing for expected errors. Access `result.Value` only when `result.IsSuccess`, and `result.Error` only when `result.IsFailure`.

## Error definitions
Errors are defined as static fields in `*Errors` classes per feature (e.g. `TournamentTemplateErrors`, `UserErrors`). Each `Error` has a code string and a human-readable message. Controllers map errors to HTTP responses via `this.ToActionResult(result.Error)`.

# Persistence
## Entity relationships
There are three types of relationships, each configured differently:

### 1. Aggregate-internal 1:N (parent owns children)
Configured on the **parent side**.

The parent uses `HasMany` / `HasOne` to express ownership. The child configuration defines only the child table, key, scalar properties, indexes, and constraints — it does not repeat the relationship.

Example: `TournamentConfiguration` configures `Tournament → Rounds` via `HasMany`. `RoundConfiguration` does not reference this relationship.

### 2. 1:1 ownership (entity belongs exclusively to another)
Configured on the **owner side**.

Example: `UserConfiguration` configures `User → TournamentTemplate` via `HasOne<TournamentTemplate>().WithOne()`. `TournamentTemplateConfiguration` does not repeat this relationship.

### 3. Cross-aggregate FK reference (N:1 reference to an external aggregate)
Configured on the **referencing entity's side**.

When an entity holds a FK to an entity outside its own aggregate, the relationship is configured in the referencing entity's configuration file.

Example: `TournamentConfiguration` configures `Tournament.OrganizerId → User` via `HasOne<User>().WithMany()`. `UserConfiguration` does not repeat this relationship.
