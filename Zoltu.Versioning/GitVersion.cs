using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Zoltu.Versioning
{
	public static class GitVersion
	{
		public static Version GetVersionFromGit(LibGit2Sharp.IRepository repository)
		{
			Contract.Ensures(Contract.Result<Version>() != null);

			var tags = new Tags(repository);
			var commits = GetHeadCommitsFromRepository(repository);

			var versionTag = TryGetVersionTagFromCommits(commits, tags);
			var commitCount = GetCommitCountSinceTag(commits, versionTag);

			var majorVersion = (versionTag != null)
				? Int32.Parse(versionTag.MajorVersion)
				: 0;
			var minorVersion = (versionTag != null)
				? Int32.Parse(versionTag.MinorVersion)
				: 0;

			if (majorVersion < 0)
				throw new ArgumentOutOfRangeException("Major version number found in tag was less than 0.  " + majorVersion);

			if (minorVersion < 0)
				throw new ArgumentOutOfRangeException("Minor version number found in tag was less than 0.  " + minorVersion);

			return new Version(majorVersion, minorVersion, commitCount, 0);
		}

		public static Int32 GetCommitCountSinceTag(IEnumerable<LibGit2Sharp.Commit> commits, VersionTag versionTag)
		{
			Contract.Requires(commits != null);
			Contract.Ensures(Contract.Result<Int32>() >= 0);

			if (versionTag == null)
				return commits.Count();

			return commits
				.Where(commit => commit != null)
				.TakeWhile(commit => commit.Sha != versionTag.Sha)
				.Count();
		}

		public static VersionTag TryGetVersionTagFromCommits(IEnumerable<LibGit2Sharp.Commit> commits, Tags tags)
		{
			if (commits == null)
				return null;

			if (tags == null)
				return null;

			return commits
				.Select(tags.TryGet)
				.Select(VersionTag.TryCreateVersionTag)
				.Where(versionTag => versionTag != null)
				.FirstOrDefault();
		}

		public static String GenerateVersionFileContents(Version assemblyVersion, Version fileVersion, Version assemblyInformationalVersion)
		{
			Contract.Requires(assemblyVersion != null);
			Contract.Requires(fileVersion != null);
			Contract.Ensures(Contract.Result<String>() != null);

			var builder = new StringBuilder();
			builder.AppendLine(@"// This is a generated file.  Do not commit it to version control and do not modify it.");
			builder.AppendLine(@"using System.Reflection;");
			builder.AppendFormat(@"[assembly: AssemblyVersion(""{0}"")]{1}", assemblyVersion, Environment.NewLine);
			builder.AppendFormat(@"[assembly: AssemblyFileVersion(""{0}"")]{1}", fileVersion, Environment.NewLine);
			builder.AppendFormat(@"[assembly: AssemblyInformationalVersion(""{0}"")]{1}", assemblyInformationalVersion, Environment.NewLine);
			return builder.ToString();
		}

		private static IEnumerable<LibGit2Sharp.Commit> GetHeadCommitsFromRepository(LibGit2Sharp.IRepository repository)
		{
			Contract.Ensures(Contract.Result<IEnumerable<LibGit2Sharp.Commit>>() != null);

			if (repository == null)
				return new List<LibGit2Sharp.Commit>();

			var head = repository.Head;
			if (head == null)
				return new List<LibGit2Sharp.Commit>();

			var commits = head.Commits;
			if (commits == null)
				return new List<LibGit2Sharp.Commit>();

			return commits;
		}
	}
}
