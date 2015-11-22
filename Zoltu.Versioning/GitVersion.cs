using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Zoltu.Collections.Generic.NotNull;
using Zoltu.Linq.NotNull;

namespace Zoltu.Versioning
{
	public static class GitVersion
	{
		public static INotNullEnumerable<String> GetHeadCommitsFromRepository(LibGit2Sharp.IRepository repository)
		{
			Contract.Ensures(Contract.Result<INotNullEnumerable<String>>() != null);

			if (repository == null)
				return EmptyEnumerable<String>.Instance;

			var head = repository.Head;
			if (head == null)
				return EmptyEnumerable<String>.Instance;

			var commits = head.Commits;
			if (commits == null)
				return EmptyEnumerable<String>.Instance;

			return commits
				.NotNull()
				.Select(commit => commit.Sha);
		}

		public static ILookup<String, Tag> GetTagsFromRepository(LibGit2Sharp.IRepository repository)
		{
			Contract.Ensures(Contract.Result<ILookup<String, Tag>>() != null);

			if (repository == null)
				return Enumerable.Empty<Tag>().ToLookup(tag => tag.Sha);
			if (repository.Tags == null)
				return Enumerable.Empty<Tag>().ToLookup(tag => tag.Sha);

			return repository.Tags
				.Where(tag => tag != null)
				.Where(tag => tag.Name != null)
				.Where(tag => tag.Target != null)
				.Where(tag => tag.Target.Sha != null)
				.Select(tag => new Tag(tag.Name, tag.Target.Sha))
				.ToLookup(tag => tag.Sha);
		}

		public static String GenerateFileContents(VersionConfiguration configuration, LibGit2Sharp.IRepository repository)
		{
			Contract.Requires(configuration != null);
			Contract.Ensures(Contract.Result<String>() != null);

			var commits = GetHeadCommitsFromRepository(repository);
			var tags = GetTagsFromRepository(repository);
			var versions = VersionFinder.GetVersions(commits, tags);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);
			return VersionFileGenerator.GenerateFileContents(versionConfigurationApplicator.Version, versionConfigurationApplicator.FileVersion, versionConfigurationApplicator.InfoVersion);
		}
	}
}
