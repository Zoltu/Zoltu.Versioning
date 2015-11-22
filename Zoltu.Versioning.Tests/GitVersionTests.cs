using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Xunit;
using Zoltu.Collections.Generic.NotNull;
using Zoltu.Linq.NotNull;

namespace Zoltu.Versioning.Tests
{
	public class GitVersionTests
	{
		[Fact]
		public void commits_when_repository_is_null()
		{
			// arrange
			var repository = null as LibGit2Sharp.IRepository;

			// act
			var commits = GitVersion.GetHeadCommitsFromRepository(repository);

			// assert
			Assert.NotNull(commits);
			Assert.Equal(0, commits.Count());
		}

		[Fact]
		public void commits_when_head_is_null()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head).Returns<LibGit2Sharp.Branch>(null);

			// act
			var commits = GitVersion.GetHeadCommitsFromRepository(repository);

			// assert
			Assert.NotNull(commits);
			Assert.Equal(0, commits.Count());
		}

		[Fact]
		public void commits_when_head_commits_is_null()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns<LibGit2Sharp.ICommitLog>(null);

			// act
			var commits = GitVersion.GetHeadCommitsFromRepository(repository);

			// assert
			Assert.NotNull(commits);
			Assert.Equal(0, commits.Count());
		}

		[Fact]
		public void commits_when_commits_found()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			var commit1 = Mock.Create<LibGit2Sharp.Commit>();
			var commit2 = Mock.Create<LibGit2Sharp.Commit>();
			var commit3 = Mock.Create<LibGit2Sharp.Commit>();
			Mock.Arrange(() => commit1.Sha).Returns("first");
			Mock.Arrange(() => commit2.Sha).Returns("second");
			Mock.Arrange(() => commit3.Sha).Returns("third");
			Mock.Arrange(() => repository.Head.Commits.GetEnumerator()).Returns(new List<LibGit2Sharp.Commit> { commit1, commit2, commit3 }.GetEnumerator());

			// act
			var commits = GitVersion.GetHeadCommitsFromRepository(repository).ToList();

			// assert
			Assert.NotNull(commits);
			Assert.Equal(3, commits.Count());
			Assert.Equal("first", commits[0]);
			Assert.Equal("second", commits[1]);
			Assert.Equal("third", commits[2]);
		}

		[Fact]
		public void tags_when_repository_is_null()
		{
			// arrange
			var repository = null as LibGit2Sharp.IRepository;

			// act
			var tags = GitVersion.GetTagsFromRepository(repository);

			// assert
			Assert.NotNull(tags);
			Assert.Equal(0, tags.Count());
		}

		[Fact]
		public void tags_when_tags_is_null()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Tags).Returns(null as LibGit2Sharp.TagCollection);

			// act
			var tags = GitVersion.GetTagsFromRepository(repository);

			// assert
			Assert.NotNull(tags);
			Assert.Equal(0, tags.Count());
		}

		[Fact]
		public void GenerateFileContents_when_default_mock()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""0.0.0.0"")]
[assembly: AssemblyFileVersion(""0.0.0.0"")]
[assembly: AssemblyInformationalVersion(""0.0.0.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_when_commits_without_tags()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first" }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""0.0.3.0"")]
[assembly: AssemblyFileVersion(""0.0.3.0"")]
[assembly: AssemblyInformationalVersion(""0.0.3.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_when_commits_and_tags()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first", "zero" }));
			Mock.Arrange(() => repository.Tags).Returns(MockTags(new Dictionary<String, String>{ {"zero", "v1.2"} }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.0"")]
[assembly: AssemblyFileVersion(""1.2.3.0"")]
[assembly: AssemblyInformationalVersion(""1.2.3.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_tag_without_sha()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first", "zero" }));
			Mock.Arrange(() => repository.Tags).Returns(MockTags(new Dictionary<String, String> { { "doesnotexist", "v1.2" } }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""0.0.4.0"")]
[assembly: AssemblyFileVersion(""0.0.4.0"")]
[assembly: AssemblyInformationalVersion(""0.0.4.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_non_version_tag()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first", "zero" }));
			Mock.Arrange(() => repository.Tags).Returns(MockTags(new Dictionary<String, String> { { "zero", "1.2" } }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""0.0.4.0"")]
[assembly: AssemblyFileVersion(""0.0.4.0"")]
[assembly: AssemblyInformationalVersion(""0.0.4.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_multiple_tags()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first", "zero" }));
			Mock.Arrange(() => repository.Tags).Returns(MockTags(new Dictionary<String, String> { { "zero", "v1.2" }, { "second", "v1.3"} }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.3.1.0"")]
[assembly: AssemblyFileVersion(""1.3.1.0"")]
[assembly: AssemblyInformationalVersion(""1.3.1.0"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void GenerateFileContents_prerelease_tag()
		{
			// arrange
			var repository = Mock.Create<LibGit2Sharp.IRepository>();
			Mock.Arrange(() => repository.Head.Commits).Returns(MockCommitLog(new[] { "third", "second", "first", "zero" }));
			Mock.Arrange(() => repository.Tags).Returns(MockTags(new Dictionary<String, String> { { "zero", "v1.2" }, { "second", "v1.3-RC" } }));
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.0"")]
[assembly: AssemblyFileVersion(""1.2.3.0"")]
[assembly: AssemblyInformationalVersion(""1.3.0.0-RC-001"")]
".Replace("\r\n", "\n")
 .Replace("\n", Environment.NewLine);

			// act
			var actualContents = GitVersion.GenerateFileContents(configuration, repository);

			// assert
			Assert.Equal(expectedContents, actualContents);
		}

		private static LibGit2Sharp.ICommitLog MockCommitLog(IEnumerable<String> commitShas)
		{
			var commitLog = Mock.Create<LibGit2Sharp.ICommitLog>();
			var mockCommits = commitShas.Select(MockCommit);
			Mock.Arrange(() => commitLog.GetEnumerator()).Returns(mockCommits.GetEnumerator());
			return commitLog;
		}

		private static LibGit2Sharp.TagCollection MockTags(IDictionary<String, String> tagShaToName)
		{
			var tagCollection = Mock.Create<LibGit2Sharp.TagCollection>();
			var mockTags = tagShaToName.Select(shaAndName => MockTag(shaAndName.Key, shaAndName.Value));
			Mock.Arrange(() => tagCollection.GetEnumerator()).Returns(mockTags.GetEnumerator());
			return tagCollection;
		}

		private static LibGit2Sharp.Commit MockCommit(String commitSha)
		{
			var mockCommit = Mock.Create<LibGit2Sharp.Commit>();
			Mock.Arrange(() => mockCommit.Sha).Returns(commitSha);
			return mockCommit;
		}

		private static LibGit2Sharp.Tag MockTag(String targetSha, String name)
		{
			var mockTag = Mock.Create<LibGit2Sharp.Tag>();
			Mock.Arrange(() => mockTag.Target.Sha).Returns(targetSha);
			Mock.Arrange(() => mockTag.Name).Returns(name);
			return mockTag;
		}
	}
}
