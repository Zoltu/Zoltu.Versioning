using System.Diagnostics.Contracts;

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
}
