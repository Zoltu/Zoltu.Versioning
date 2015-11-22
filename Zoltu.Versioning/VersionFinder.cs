using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Versioning
{
	public static class VersionFinder
	{
		public static Versions GetVersions(INotNullEnumerable<String> commits, ILookup<String, Tag> commitsToTags)
		{
			Contract.Requires(commits != null);
			Contract.Requires(commitsToTags != null);
			Contract.Ensures(Contract.Result<Versions>() != null);

			var commitsSinceReleaseVersionTag = 0;
			var commitsSincePrereleaseVersionTag = 0;
			var releaseVersionTag = null as VersionTag;
			var prereleaseVersionTag = null as VersionTag;

			foreach (var commit in commits)
			{
				var tags = commitsToTags[commit];
				foreach (var tag in tags)
				{
					if (tag == null)
						continue;

					var versionTag = VersionTag.TryCreate(tag);
					if (versionTag == null)
						continue;

					if (versionTag.Suffix != null)
					{
						if (prereleaseVersionTag == null)
							prereleaseVersionTag = versionTag;
					}
					else
					{
						releaseVersionTag = versionTag;
						break;
					}
				}

				if (releaseVersionTag != null)
					break;

				++commitsSinceReleaseVersionTag;
				if (prereleaseVersionTag == null)
					++commitsSincePrereleaseVersionTag;
			}

			var releaseVersion = releaseVersionTag != null
				? new Version(releaseVersionTag.MajorVersion, releaseVersionTag.MinorVersion, commitsSinceReleaseVersionTag, 0)
				: new Version(0, 0, commitsSinceReleaseVersionTag, 0);
			var prereleaseVersion = prereleaseVersionTag != null
				? new Version(prereleaseVersionTag.MajorVersion, prereleaseVersionTag.MinorVersion, 0, 0, String.Format("{0}-{1}", prereleaseVersionTag.Suffix, commitsSincePrereleaseVersionTag.ToString("D3")))
				: releaseVersion;

			return new Versions(releaseVersion, releaseVersion, prereleaseVersion);
		}

	}
}
