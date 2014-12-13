using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zoltu.Linq.NotNull;

namespace Zoltu.Versioning.Tests
{
	public class VersionFinderTests
	{
		[Fact]
		public void release_and_prerelease_tags()
		{
			// arrange
			var commits = new List<String>
			{
				"sixth",
				"fifth",
				"fourth",
				"third",
				"second",
				"first",
				"zero",
			}.NotNull();
			var tags = new List<Tag>
			{
				new Tag("v1.2", "zero"),
				new Tag("v1.3-RC", "third"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("1.2.6.0", versions.Version.ToString());
			Assert.Equal("1.2.6.0", versions.FileVersion.ToString());
			Assert.Equal("1.3.0.0-RC-003", versions.InfoVersion.ToString());
		}

		[Fact]
		public void release_tag_only()
		{
			// arrange
			var commits = new List<String>
			{
				"sixth",
				"fifth",
				"fourth",
				"third",
				"second",
				"first",
				"zero",
			}.NotNull();
			var tags = new List<Tag>
			{
				new Tag("v1.2", "zero"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("1.2.6.0", versions.Version.ToString());
			Assert.Equal("1.2.6.0", versions.FileVersion.ToString());
			Assert.Equal("1.2.6.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void prerelease_tag_only()
		{
			// arrange
			var commits = new List<String>
			{
				"sixth",
				"fifth",
				"fourth",
				"third",
				"second",
				"first",
				"zero",
			}.NotNull();
			var tags = new List<Tag>
			{
				new Tag("v1.3-RC", "third"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("0.0.7.0", versions.Version.ToString());
			Assert.Equal("0.0.7.0", versions.FileVersion.ToString());
			Assert.Equal("1.3.0.0-RC-003", versions.InfoVersion.ToString());
		}

		[Fact]
		public void multiple_release_tags()
		{
			// arrange
			var commits = new List<String>
			{
				"sixth",
				"fifth",
				"fourth",
				"third",
				"second",
				"first",
				"zero",
			}.NotNull();
			var tags = new List<Tag>
			{
				new Tag("v1.2", "zero"),
				new Tag("v1.3-RC", "third"),
				new Tag("v1.3", "fifth"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("1.3.1.0", versions.Version.ToString());
			Assert.Equal("1.3.1.0", versions.FileVersion.ToString());
			Assert.Equal("1.3.1.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void no_commits()
		{
			// arrange
			var commits = new List<String>().NotNull();
			var tags = new List<Tag>().ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("0.0.0.0", versions.Version.ToString());
			Assert.Equal("0.0.0.0", versions.FileVersion.ToString());
			Assert.Equal("0.0.0.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void one_commit_no_tags()
		{
			// arrange
			var commits = new List<String>{"first"}.NotNull();
			var tags = new List<Tag>().ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("0.0.1.0", versions.Version.ToString());
			Assert.Equal("0.0.1.0", versions.FileVersion.ToString());
			Assert.Equal("0.0.1.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void one_commit_one_tag()
		{
			// arrange
			var commits = new List<String>{"first"}.NotNull();
			var tags = new List<Tag>{new Tag("v1.0", "first")}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("1.0.0.0", versions.Version.ToString());
			Assert.Equal("1.0.0.0", versions.FileVersion.ToString());
			Assert.Equal("1.0.0.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void multiple_tags_same_commit()
		{
			// arrange
			var commits = new List<String> { "first" }.NotNull();
			var tags = new List<Tag>
			{
				new Tag("foo", "first"),
				new Tag("v1.0", "first"),
				new Tag("bar", "first"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("1.0.0.0", versions.Version.ToString());
			Assert.Equal("1.0.0.0", versions.FileVersion.ToString());
			Assert.Equal("1.0.0.0", versions.InfoVersion.ToString());
		}

		[Fact]
		public void multiple_prerelease_tags_same_commit()
		{
			// arrange
			var commits = new List<String>
			{
				"second",
				"first",
			}.NotNull();
			var tags = new List<Tag>
			{
				new Tag("foo", "first"),
				new Tag("v1.0-RC", "first"),
				new Tag("bar", "first"),
			}.ToLookup(tag => tag.Sha);
			var configuration = new VersionConfiguration(true, true, true);

			// act
			var versions = VersionFinder.GetVersions(configuration, commits, tags);

			// assert
			Assert.Equal("0.0.2.0", versions.Version.ToString());
			Assert.Equal("0.0.2.0", versions.FileVersion.ToString());
			Assert.Equal("1.0.0.0-RC-001", versions.InfoVersion.ToString());
		}
	}
}
