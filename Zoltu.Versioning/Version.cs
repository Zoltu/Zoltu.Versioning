using System;
using System.Diagnostics.Contracts;

namespace Zoltu.Versioning
{
	public class Version
	{
		public Int32 Major { get; private set; }
		public Int32 Minor { get; private set; }
		public Int32 Patch { get; private set; }
		public Int32 Build { get; private set; }
		public String Suffix { get; private set; }

		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Major >= 0);
			Contract.Invariant(Minor >= 0);
			Contract.Invariant(Patch >= 0);
			Contract.Invariant(Build >= 0);
		}

		public static Version CreateVersion(VersionTag versionTag, Int32 commitCount)
		{
			Contract.Requires(commitCount >= 0);
			Contract.Ensures(Contract.Result<Version>() != null);

			if (versionTag == null)
				return new Version(0, 0, commitCount, 0, null);

			var suffix = (versionTag.Suffix != null)
				? versionTag.Suffix + "-" + commitCount.ToString("D4")
				: null;
			var patchVersion = commitCount;
			var minorVersion = versionTag.MinorVersion;
			var majorVersion = versionTag.MajorVersion;

			return new Version(majorVersion, minorVersion, patchVersion, 0, suffix);
		}

		public Version(Int32 major, Int32 minor, Int32 patch, Int32 build, String suffix = null)
		{
			Contract.Requires(major >= 0);
			Contract.Requires(minor >= 0);
			Contract.Requires(patch >= 0);
			Contract.Requires(build >= 0);

			Major = major;
			Minor = minor;
			Patch = patch;
			Build = build;
			Suffix = suffix;
		}

		public override string ToString()
		{
			var suffix = (Suffix != null)
				? "-" + Suffix
				: String.Empty;
			return String.Format(@"{0}.{1}.{2}.{3}{4}", Major, Minor, Patch, Build, suffix);
		}
	}
}
