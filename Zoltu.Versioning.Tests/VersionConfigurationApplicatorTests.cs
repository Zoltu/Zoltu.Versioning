using Xunit;

namespace Zoltu.Versioning.Tests
{
	public class VersionConfigurationApplicatorTests
	{
		[Fact]
		public void all_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true, true, true, true);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);

			// act
			var cleanedVersion = versionConfigurationApplicator.Version;
			var cleanedFileVersion = versionConfigurationApplicator.FileVersion;
			var cleanedInfoVersion = versionConfigurationApplicator.InfoVersion;

			// assert
			Assert.Equal("1.2.3.4-alpha", cleanedVersion);
			Assert.Equal("3.4.5.6-beta", cleanedFileVersion);
			Assert.Equal("5.6.7.8-delta", cleanedInfoVersion);
		}

		[Fact]
		public void no_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true, false, false, false);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);

			// act
			var cleanedVersion = versionConfigurationApplicator.Version;
			var cleanedFileVersion = versionConfigurationApplicator.FileVersion;
			var cleanedInfoVersion = versionConfigurationApplicator.InfoVersion;

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersion);
			Assert.Equal("3.4.0.0-beta", cleanedFileVersion);
			Assert.Equal("5.6.0.0-delta", cleanedInfoVersion);
		}

		[Fact]
		public void primary_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true, true, false, false);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);

			// act
			var cleanedVersion = versionConfigurationApplicator.Version;
			var cleanedFileVersion = versionConfigurationApplicator.FileVersion;
			var cleanedInfoVersion = versionConfigurationApplicator.InfoVersion;

			// assert
			Assert.Equal("1.2.3.4-alpha", cleanedVersion);
			Assert.Equal("3.4.0.0-beta", cleanedFileVersion);
			Assert.Equal("5.6.0.0-delta", cleanedInfoVersion);
		}

		[Fact]
		public void file_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true, false, true, false);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);

			// act
			var cleanedVersion = versionConfigurationApplicator.Version;
			var cleanedFileVersion = versionConfigurationApplicator.FileVersion;
			var cleanedInfoVersion = versionConfigurationApplicator.InfoVersion;

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersion);
			Assert.Equal("3.4.5.6-beta", cleanedFileVersion);
			Assert.Equal("5.6.0.0-delta", cleanedInfoVersion);
		}

		[Fact]
		public void info_patch()
		{
			// arrange
			var version = new Version(1, 2, 3, 4, "alpha");
			var fileVersion = new Version(3, 4, 5, 6, "beta");
			var infoVersion = new Version(5, 6, 7, 8, "delta");
			var versions = new Versions(version, fileVersion, infoVersion);
			var configuration = new VersionConfiguration(true, true, true, false, false, true);
			var versionConfigurationApplicator = new VersionConfigurationApplicator(versions, configuration);

			// act
			var cleanedVersion = versionConfigurationApplicator.Version;
			var cleanedFileVersion = versionConfigurationApplicator.FileVersion;
			var cleanedInfoVersion = versionConfigurationApplicator.InfoVersion;

			// assert
			Assert.Equal("1.2.0.0-alpha", cleanedVersion);
			Assert.Equal("3.4.0.0-beta", cleanedFileVersion);
			Assert.Equal("5.6.7.8-delta", cleanedInfoVersion);
		}
	}
}
