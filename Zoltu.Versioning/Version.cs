using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoltu.Versioning
{
	public class Version
	{
		public readonly Int32 Major;
		public readonly Int32 Minor;
		public readonly Int32 Patch;
		public readonly Int32 Build;
		public readonly String Suffix;

		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Major >= 0);
			Contract.Invariant(Minor >= 0);
			Contract.Invariant(Patch >= 0);
			Contract.Invariant(Build >= 0);
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
