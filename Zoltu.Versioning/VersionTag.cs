using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Zoltu.Versioning
{
	public class VersionTag : Tag
	{
		private static Regex tagMatcher = new Regex(@"^v(\d+)\.(\d+)", RegexOptions.Compiled);

		public String MajorVersion
		{
			get
			{
				Contract.Ensures(Contract.Result<String>() != null);
				return _majorVersion;
			}
		}
		private readonly String _majorVersion;

		public String MinorVersion
		{
			get
			{
				Contract.Ensures(Contract.Result<String>() != null);
				return _minorVersion;
			}
		}
		private readonly String _minorVersion;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_majorVersion != null);
			Contract.Invariant(_minorVersion != null);
		}

		public VersionTag(Tag tag, String majorVersion, String minorVersion) : base(tag.Name, tag.Sha)
		{
			Contract.Requires(tag != null);
			Contract.Requires(majorVersion != null);
			Contract.Requires(minorVersion != null);

			_majorVersion = majorVersion;
			_minorVersion = minorVersion;
		}

		public static VersionTag TryCreateVersionTag(Tag tag)
		{
			if (tag == null)
				return null;

			var match = tagMatcher.Match(tag.Name);
			if (!match.Success)
				return null;

			if (match.Groups.Count != 3)
				return null;

			var majorVersion = match.Groups[1];
			var minorVersion = match.Groups[2];
			return new VersionTag(tag, majorVersion.Value, minorVersion.Value);
		}
	}
}
