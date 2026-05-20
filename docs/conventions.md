# Testing
## Naming
The name of test methods is constructed as follows:
`<name of method>_<state / condition>_<expected result>`


The expected result is written in active tense, i.e. "IsCompleted" rather than "ShouldBeCompleted" or "ThrowsArgumentsException" rather than "ShouldThrowArgumentException".

Example: Tournament.Constructor_WithEmptyOrganizerId_ThrowsArgumentException()

## Constructor tests
Successful constructor tests verify explicitly provided values only.

Default values are tested in separate test methods.

# Persistence
## Entity relationships
Entity relationships are configured on the parent side only.

For example, the relationship between tournaments and rounds is configured in
`TournamentConfiguration.cs`, not in `RoundConfiguration.cs`.

Child entity configurations define the child table, key, scalar properties,
indexes, and constraints, but do not repeat relationships that are already
owned by the parent configuration.
