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
				var version = GitVersion.GetVersionFromGit(repository);
				var versionNoSuffix = new Version(version.Major, version.Minor, version.Patch, 0, null);
				var majorAndMinorOnly = new Version(version.Major, version.Minor, 0, 0, version.Suffix);
				var majorAndMinorOnlyNoSuffix = new Version(version.Major, version.Minor, 0, 0, null);

				var assemblyVersion = (OnlyMajorAndMinorInAssemblyVersion)
					? majorAndMinorOnlyNoSuffix
					: versionNoSuffix;
				var assemblyFileVersion = (OnlyMajorAndMinorInAssemblyFileVersion)
					? majorAndMinorOnlyNoSuffix
					: versionNoSuffix;
				var assemblyInformationalVersion = (OnlyMajorAndMinorInAssemblyInformationalVersion)
					? majorAndMinorOnly
					: version;

				var fileContents = GitVersion.GenerateVersionFileContents(assemblyVersion, assemblyFileVersion, assemblyInformationalVersion);
				File.WriteAllText(OutputFilePath, fileContents);
			}

			return true;
		}
	}
}
