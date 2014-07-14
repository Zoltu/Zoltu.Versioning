using System;
using System.Diagnostics.Contracts;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Zoltu.Versioning
{
	public class GitTask : Task
	{
		[Required]
		public String OutputFilePath { get; set; }

		public override Boolean Execute()
		{
			Contract.Assume(Log != null);

			var repositorySearchPathStart = Path.GetDirectoryName(OutputFilePath);
			var repositoryPath = LibGit2Sharp.Repository.Discover(repositorySearchPathStart);
			var repository = (repositoryPath != null)
				? new LibGit2Sharp.Repository(repositoryPath)
				: null;
			using (repository)
			{
				var version = GitVersion.GetVersionFromGit(repository);
				var fileContents = GitVersion.GenerateVersionFileContents(version);
				File.WriteAllText(OutputFilePath, fileContents);
			}

			return true;
		}
	}
}
