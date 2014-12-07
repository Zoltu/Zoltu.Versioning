using System;

namespace Zoltu.Versioning
{
	public class VersionConfiguration
	{
		public Boolean IncludePatchAndBuildInVersion { get; private set; }
		public Boolean IncludePatchAndBuildInFileVersion { get; private set; }
		public Boolean IncludePatchAndBuildInInfoVersion { get; private set; }

		public VersionConfiguration(Boolean includePatchAndBuildInVersion, Boolean includePatchAndBuildInFileVersion, Boolean includePatchAndBuildInInfoVersion)
		{
			IncludePatchAndBuildInVersion = includePatchAndBuildInVersion;
			IncludePatchAndBuildInFileVersion = includePatchAndBuildInFileVersion;
			IncludePatchAndBuildInInfoVersion = includePatchAndBuildInInfoVersion;
		}
	}
}
