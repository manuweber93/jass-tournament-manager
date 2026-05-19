# Testing
## Naming
The name of test methods is constructed as follows:
`<name of method>_<state / condition>_<expected result>`


The expected result is written in active tense, i.e. "IsCompleted" rather than "ShouldBeCompleted" or "ThrowsArgumentsException" rather than "ShouldThrowArgumentException".

Example: Tournament.Constructor_WithEmptyOrganizerId_ThrowsArgumentException()

## Constructor tests
Successful constructor tests verify explicitly provided values only.

Default values are tested in separate test methods.
