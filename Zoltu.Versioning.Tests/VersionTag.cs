using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Zoltu.Versioning.Tests
{
	public class VersionTagTests
	{
		[Theory]
		[InlineData("foo")]
		[InlineData("v1")]
		[InlineData("v1.")]
		[InlineData("va1.2")]
		[InlineData("av1.2")]
		[InlineData("v1.2a")]
		[InlineData("v1.2-")]
		[InlineData("v1.-2")]
		[InlineData("v-1.2")]
		public void null_version_tags(String tagName)
		{
			// arrange
			var tag = new Tag(tagName, "");

			// act
			var versionTag = VersionTag.TryCreate(tag);

			// assert
			Assert.Null(versionTag);
		}

		[Theory]
		[InlineData("v1.2", 1, 2, null)]
		[InlineData("v1.2-alpha", 1, 2, "alpha")]
		[InlineData("v1.2-alpha5", 1, 2, "alpha5")]
		[InlineData("v1.2-betav1.5", 1, 2, "betav1.5")]
		public void valid_version_tags(String tagName, Int32 majorVersion, Int32 minorVersion, String suffix)
		{
			// arrange
			var tag = new Tag(tagName, "");

			// act
			var versionTag = VersionTag.TryCreate(tag);

			// assert
			Assert.Equal(majorVersion, versionTag.MajorVersion);
			Assert.Equal(minorVersion, versionTag.MinorVersion);
			Assert.Equal(suffix, versionTag.Suffix);
		}
	}
}
