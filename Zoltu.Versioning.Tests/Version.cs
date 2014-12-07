using System;
using Xunit;
using Xunit.Extensions;

namespace Zoltu.Versioning.Tests
{
	public class VersionTests
	{
		[Theory]
		[InlineData("v1.1", 2, "1.1.2.0")]
		public void valid_version_from_version_tag_and_commit_count(String tagName, Int32 commitCount, String expectedVersion)
		{
			// arrange
			var tag = new Tag(tagName, "");

			// act
			var versionTag = VersionTag.TryCreate(tag);
			var actualVersion = Version.CreateVersion(versionTag, commitCount);

			// assert
			Assert.Equal(actualVersion.ToString(), expectedVersion);
		}
	}
}
