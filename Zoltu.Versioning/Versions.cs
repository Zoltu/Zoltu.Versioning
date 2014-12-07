using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoltu.Versioning
{
	public class Versions
	{
		public Version Version { get; private set; }
		public Version FileVersion { get; private set; }
		public Version InfoVersion { get; private set; }

		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Version != null);
			Contract.Invariant(FileVersion != null);
			Contract.Invariant(InfoVersion != null);
		}

		public Versions(Version version, Version fileVersion, Version infoVersion)
		{
			Contract.Requires(version != null);
			Contract.Requires(fileVersion != null);
			Contract.Requires(infoVersion != null);

			Version = version;
			FileVersion = fileVersion;
			InfoVersion = infoVersion;
		}
	}

	public static class VersionsExtensions
	{
		public static Versions ApplyConfiguration(this Versions versions, VersionConfiguration configuration)
		{
			Contract.Requires(versions != null);
			Contract.Requires(configuration != null);
			Contract.Ensures(Contract.Result<Versions>() != null);

			var version = (configuration.IncludePatchAndBuildInVersion)
				? versions.Version
				: new Version(versions.Version.Major, versions.Version.Minor, 0, 0, versions.Version.Suffix);

			var fileVersion = (configuration.IncludePatchAndBuildInFileVersion)
				? versions.FileVersion
				: new Version(versions.FileVersion.Major, versions.FileVersion.Minor, 0, 0, versions.FileVersion.Suffix);

			var infoVersion = (configuration.IncludePatchAndBuildInInfoVersion)
				? versions.InfoVersion
				: new Version(versions.InfoVersion.Major, versions.InfoVersion.Minor, 0, 0, versions.InfoVersion.Suffix);

			return new Versions(version, fileVersion, infoVersion);
		}
	}
}
