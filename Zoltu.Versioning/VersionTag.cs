using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Zoltu.Versioning
{
	public class VersionTag : Tag
	{
		private static Regex tagMatcher = new Regex(@"^v(?<Major>\d+).(?<Minor>\d+)(?:-(?<Suffix>.+))?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		public Int32 MajorVersion
		{
			get
			{
				Contract.Ensures(Contract.Result<Int32>() >= 0);
				return _majorVersion;
			}
		}
		private readonly Int32 _majorVersion;

		public Int32 MinorVersion
		{
			get
			{
				Contract.Ensures(Contract.Result<Int32>() >= 0);
				return _minorVersion;
			}
		}
		private readonly Int32 _minorVersion;

		public String Suffix
		{
			get
			{
				return _suffix;
			}
		}
		private readonly String _suffix;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_majorVersion >= 0);
			Contract.Invariant(_minorVersion >= 0);
		}

		public VersionTag(Tag tag, Int32 majorVersion, Int32 minorVersion, String suffix) : base(tag.Name, tag.Sha)
		{
			Contract.Requires(tag != null);
			Contract.Requires(majorVersion >= 0);
			Contract.Requires(minorVersion >= 0);

			_majorVersion = majorVersion;
			_minorVersion = minorVersion;
			_suffix = suffix;
		}

		public static VersionTag TryCreateVersionTag(Tag tag)
		{
			try
			{
				if (tag == null)
					return null;

				var match = tagMatcher.Match(tag.Name);
				if (!match.Success)
					return null;

				var majorVersion = GetVersionFromMatch(match, "Major");
				var minorVersion = GetVersionFromMatch(match, "Minor");
				var suffix = TryGetSuffixFromMatch(match);

				return new VersionTag(tag, majorVersion, minorVersion, suffix);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static Int32 GetVersionFromMatch(Match match, String groupName)
		{
			Contract.Ensures(Contract.Result<Int32>() >= 0);

			var valueGroup = match.Groups[groupName];
			if (!valueGroup.Success)
				throw new Exception(String.Format("Expected match {0} but did not find one.", groupName));
			
			var valueString = valueGroup.Value;
			if (String.IsNullOrWhiteSpace(valueString))
				throw new Exception(String.Format("Expected non empty string for match group {0}.", groupName));

			var valueInt32 = Int32.Parse(valueString);
			if (valueInt32 < 0)
				throw new Exception(String.Format("Expected positive Int32 value for match group {0} but found {1}", groupName, valueInt32));

			return valueInt32;
		}

		private static String TryGetSuffixFromMatch(Match match)
		{
			var valueGroup = match.Groups["Suffix"];
			if (!valueGroup.Success)
				return null;

			return valueGroup.Value;
		}
	}
}
