using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Zoltu.Versioning
{
	public class Git : Task
	{
		[Required]
		public String OutputFilePath { get; set; }

		private static Regex tagMatcher = new Regex(@"^v(\d+)\.(\d+)", RegexOptions.Compiled);

		public override Boolean Execute()
		{
			Contract.Assume(Log != null);

			var repositorySearchPathStart = Path.GetDirectoryName(OutputFilePath);
			var version = GetVersionFromGit(repositorySearchPathStart);
			var fileContents = GenerateVersionFileContents(version);
			File.WriteAllText(OutputFilePath, fileContents);

			return true;
		}

		public static String GetVersionFromGit(String repositorySearchPathStart)
		{
			Contract.Requires(repositorySearchPathStart != null);

			var repositoryPath = LibGit2Sharp.Repository.Discover(repositorySearchPathStart);
			using (var repository = new LibGit2Sharp.Repository(repositoryPath))
			{
				var tag = GetVersionTagFromGit(repository);
				var commitCount = (tag == null)
					? GetCommitCountSinceStart(repository)
					: GetCommitCountSinceTag(repository, tag);

				var tagMatch = tagMatcher.Match(tag.Name);
				var majorVersion = tagMatch.Groups[1].Value;
				var minorVersion = tagMatch.Groups[2].Value;

				return String.Format(@"{0}.{1}.{2}.0", majorVersion, minorVersion, commitCount);
			}
		}

		public static Int32 GetCommitCountSinceStart(LibGit2Sharp.IRepository repository)
		{
			Contract.Requires(repository != null);
			Contract.Ensures(Contract.Result<Int32>() >= 0);

			return repository.Head.Commits.Count();
		}

		public static Int32 GetCommitCountSinceTag(LibGit2Sharp.IRepository repository, LibGit2Sharp.Tag tag)
		{
			Contract.Requires(repository != null);
			Contract.Requires(tag != null);
			Contract.Ensures(Contract.Result<Int32>() >= 0);

			return repository.Head.Commits
				.TakeWhile(commit => commit.Sha != tag.Target.Sha)
				.Count();
		}

		private static LibGit2Sharp.Tag GetVersionTagFromGit(LibGit2Sharp.IRepository repository)
		{
			Contract.Requires(repository != null);

			var tags = repository.Tags.ToDictionary(tag => tag.Target.Sha);

			return repository.Head.Commits
				.Where(commit => tags.ContainsKey(commit.Sha))
				.Select(commit => tags[commit.Sha])
				.Where(tag => tagMatcher.IsMatch(tag.Name))
				.FirstOrDefault();
		}

		public static String GenerateVersionFileContents(String version)
		{
			Contract.Requires(version != null);
			Contract.Ensures(Contract.Result<String>() != null);

			var builder = new StringBuilder();
			builder.AppendLine(@"// This is a generated file.  Do not commit it to version control and do not modify it.");
			builder.AppendLine(@"using System.Reflection;");
			builder.AppendFormat(@"[assembly: AssemblyVersion(""{0}"")]{1}", version, Environment.NewLine);
			builder.AppendFormat(@"[assembly: AssemblyFileVersion(""{0}"")]{1}", version, Environment.NewLine);
			return builder.ToString();
		}
	}
}
