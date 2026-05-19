using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_WithBlankFirstName_ThrowsArgumentException()
        {
            var blankFirstName = " ";
            
            Action act = () => new User(
                UserTestData.CreateEmail(),
                UserTestData.CreatePasswordHash(),
                blankFirstName,
                UserTestData.CreateLastName(),
                UserTestData.CreateSourceType());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithBlankLastName_ThrowsArgumentException()
        {
            var blankLastName = " ";

            Action act = () => new User(
                UserTestData.CreateEmail(),
                UserTestData.CreatePasswordHash(),
                UserTestData.CreateFirstName(),
                blankLastName,
                UserTestData.CreateSourceType());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithSelfRegisteredUserAndBlankEmail_ThrowsArgumentException()
        {
            var blankEmail = " ";

            Action act = () => new User(
                blankEmail,
                UserTestData.CreatePasswordHash(),
                UserTestData.CreateFirstName(),
                UserTestData.CreateLastName(),
                UserSourceType.SelfRegistered);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithSelfRegisteredUserAndBlankPasswordHash_ThrowsArgumentException()
        {
            var blankPasswordHash = " ";

            Action act = () => new User(
                UserTestData.CreateEmail(),
                blankPasswordHash,
                UserTestData.CreateFirstName(),
                UserTestData.CreateLastName(),
                UserSourceType.SelfRegistered);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_DefaultIsSysAdmin_IsFalse()
        {
            var user = UserTestData.CreateUser();

            user.IsSysAdmin.Should().BeFalse();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesUser()
        {
            var email = UserTestData.CreateEmail();
            var passwordHash = UserTestData.CreatePasswordHash();
            var firstName = UserTestData.CreateFirstName();
            var lastName = UserTestData.CreateLastName();
            var sourceType = UserTestData.CreateSourceType();
            var isSysAdmin = true;

            var user = new User(
                email,
                passwordHash,
                firstName,
                lastName,
                sourceType,
                isSysAdmin);

            user.Email.Should().Be(email);
            user.PasswordHash.Should().Be(passwordHash);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.SourceType.Should().Be(sourceType);
            user.IsSysAdmin.Should().Be(isSysAdmin);
        }

        [Fact]
        public void Constructor_WithPaddedValues_NormalizesUser()
        {
            var email = UserTestData.CreateEmail();
            var passwordHash = UserTestData.CreatePasswordHash();
            var firstName = UserTestData.CreateFirstName();
            var lastName = UserTestData.CreateLastName();

            var user = new User(
                " " + email.ToUpperInvariant() + " ",
                " " + passwordHash + " ",
                " " + firstName + " ",
                " " + lastName + " ",
                UserTestData.CreateSourceType());

            user.Email.Should().Be(email);
            user.PasswordHash.Should().Be(passwordHash);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
        }

        [Fact]
        public void Constructor_WithManualUserSourceTypeAndMissingCredentials_CreatesUser()
        {
            var user = new User(
                null,
                null,
                UserTestData.CreateFirstName(),
                UserTestData.CreateLastName(),
                UserSourceType.Manual);

            user.Email.Should().BeNull();
            user.PasswordHash.Should().BeNull();
            user.SourceType.Should().Be(UserSourceType.Manual);
        }

        [Fact]
        public void MergeIntoDifferentUser_WithEmptyMergeTargetUserId_ThrowsArgumentException()
        {
            var user = UserTestData.CreateUser();
            var emptyMergeTargetUserId = Guid.Empty;

            Action act = () => user.MergeIntoDifferentUser(emptyMergeTargetUserId, Guid.NewGuid());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void MergeIntoDifferentUser_WithEmptyMergedByUserId_ThrowsArgumentException()
        {
            var user = UserTestData.CreateUser();
            var emptyMergedByUserId = Guid.Empty;

            Action act = () => user.MergeIntoDifferentUser(Guid.NewGuid(), emptyMergedByUserId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void MergeIntoDifferentUser_WithValidValues_MarksUserAsMerged()
        {
            var user = UserTestData.CreateUser();
            var mergeTargetUserId = Guid.NewGuid();
            var mergedByUserId = Guid.NewGuid();

            user.MergeIntoDifferentUser(mergeTargetUserId, mergedByUserId);

            user.MergeTargetUserId.Should().Be(mergeTargetUserId);
            user.MergedBy.Should().Be(mergedByUserId);
            user.MergedAt.Should().NotBeNull();
        }

        [Fact]
        public void MergeIntoDifferentUser_WithOwnUserId_ThrowsInvalidOperationException()
        {
            var user = UserTestData.CreateUser();

            Action act = () => user.MergeIntoDifferentUser(user.Id, Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void MergeIntoDifferentUser_WhenUserAlreadyMerged_ThrowsInvalidOperationException()
        {
            var user = UserTestData.CreateUser();
            user.MergeIntoDifferentUser(Guid.NewGuid(), Guid.NewGuid());

            Action act = () => user.MergeIntoDifferentUser(Guid.NewGuid(), Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }
    }
}
