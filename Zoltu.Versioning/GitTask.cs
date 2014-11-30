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

		public Boolean OnlyMajorAndMinorInAssemblyVersion { get; set; }

		public Boolean OnlyMajorAndMinorInAssemblyFileVersion { get; set; }

		public Boolean OnlyMajorAndMinorInAssemblyInformationalVersion { get; set; }

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
				var fileContents = GitVersion.GenerateVersionFileContents(repository, OnlyMajorAndMinorInAssemblyVersion, OnlyMajorAndMinorInAssemblyFileVersion, OnlyMajorAndMinorInAssemblyInformationalVersion);
				File.WriteAllText(OutputFilePath, fileContents);
			}

			return true;
		}
	}
}
