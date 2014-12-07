using Xunit;

namespace Zoltu.Versioning.Tests
{
	public class VersionsTests
	{
		[Fact]
		public void all_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var cleanedVersions = versions.ApplyConfiguration(configuration);

			// assert
			Assert.Equal("1.2.3.4-alpha", cleanedVersions.Version.ToString());
			Assert.Equal("3.4.5.6-beta", cleanedVersions.FileVersion.ToString());
			Assert.Equal("5.6.7.8-delta", cleanedVersions.InfoVersion.ToString());
		}

		[Fact]
		public void no_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(false, false, false);

			// act
			var cleanedVersions = versions.ApplyConfiguration(configuration);

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersions.Version.ToString());
			Assert.Equal("3.4.0.0-beta", cleanedVersions.FileVersion.ToString());
			Assert.Equal("5.6.0.0-delta", cleanedVersions.InfoVersion.ToString());
		}

		[Fact]
		public void primary_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, false, false);

			// act
			var cleanedVersions = versions.ApplyConfiguration(configuration);

			// assert
			Assert.Equal("1.2.3.4-alpha", cleanedVersions.Version.ToString());
			Assert.Equal("3.4.0.0-beta", cleanedVersions.FileVersion.ToString());
			Assert.Equal("5.6.0.0-delta", cleanedVersions.InfoVersion.ToString());
		}

		[Fact]
		public void file_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(false, true, false);

			// act
			var cleanedVersions = versions.ApplyConfiguration(configuration);

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersions.Version.ToString());
			Assert.Equal("3.4.5.6-beta", cleanedVersions.FileVersion.ToString());
			Assert.Equal("5.6.0.0-delta", cleanedVersions.InfoVersion.ToString());
		}

		[Fact]
		public void info_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(false, false, true);

			// act
			var cleanedVersions = versions.ApplyConfiguration(configuration);

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersions.Version.ToString());
			Assert.Equal("3.4.0.0-beta", cleanedVersions.FileVersion.ToString());
			Assert.Equal("5.6.7.8-delta", cleanedVersions.InfoVersion.ToString());
		}
	}
}
