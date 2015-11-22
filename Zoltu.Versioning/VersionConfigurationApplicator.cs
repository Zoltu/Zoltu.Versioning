using System;
using System.Diagnostics.Contracts;

namespace Zoltu.Versioning
{
	public class VersionConfigurationApplicator
	{
		private readonly Versions _versions;
		private readonly VersionConfiguration _configuration;

		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(_versions != null);
			Contract.Invariant(_configuration != null);
		}
		
		public VersionConfigurationApplicator(Versions versions, VersionConfiguration configuration)
		{
			Contract.Requires(versions != null);
			Contract.Requires(configuration != null);

			_versions = versions;
			_configuration = configuration;
		}

		public String Version
		{
			get
			{
				var configuredVersion = _configuration.IncludeVersion
					? _configuration.IncludePatchAndBuildInVersion
						? _versions.Version
						: new Version(_versions.Version.Major, _versions.Version.Minor, 0, 0, _versions.Version.Suffix)
					: null;

				return configuredVersion?.ToString();
			}
		}

		public String FileVersion
		{
			get
			{
				var configuredFileVersion = _configuration.IncludeFileVersion
					? _configuration.IncludePatchAndBuildInFileVersion
						? _versions.FileVersion
						: new Version(_versions.FileVersion.Major, _versions.FileVersion.Minor, 0, 0, _versions.FileVersion.Suffix)
					: null;

				return configuredFileVersion?.ToString();
			}
		}

		public String InfoVersion
		{
			get
			{
				var configuredInfoVersion = _configuration.IncludeInfoVersion
					? _configuration.IncludePatchAndBuildInInfoVersion
						? _versions.InfoVersion
						: new Version(_versions.InfoVersion.Major, _versions.InfoVersion.Minor, 0, 0, _versions.InfoVersion.Suffix)
					: null;

				return configuredInfoVersion?.ToString();
			}
		}
	}
}
