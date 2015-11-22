using System;

namespace Zoltu.Versioning
{
	public class VersionConfiguration
	{
		public Boolean IncludeVersion { get; private set; }
		public Boolean IncludeFileVersion { get; private set; }
		public Boolean IncludeInfoVersion { get; private set; }
		public Boolean IncludePatchAndBuildInVersion { get; private set; }
		public Boolean IncludePatchAndBuildInFileVersion { get; private set; }
		public Boolean IncludePatchAndBuildInInfoVersion { get; private set; }

		public VersionConfiguration(Boolean includeVersion, Boolean includeFileVersion, Boolean includeInfoVersion, Boolean includePatchAndBuildInVersion, Boolean includePatchAndBuildInFileVersion, Boolean includePatchAndBuildInInfoVersion)
		{
			IncludeVersion = includeVersion;
			IncludeFileVersion = includeFileVersion;
			IncludeInfoVersion = includeInfoVersion;
			IncludePatchAndBuildInVersion = includePatchAndBuildInVersion;
			IncludePatchAndBuildInFileVersion = includePatchAndBuildInFileVersion;
			IncludePatchAndBuildInInfoVersion = includePatchAndBuildInInfoVersion;
		}
	}
}
