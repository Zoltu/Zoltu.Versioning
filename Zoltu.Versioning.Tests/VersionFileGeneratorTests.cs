using System;
using Xunit;

namespace Zoltu.Versioning.Tests
{
	public class VersionFileGeneratorTests
	{
		[Fact]
		public void all_versions_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
[assembly: AssemblyFileVersion(""5.6.7.8"")]
[assembly: AssemblyInformationalVersion(""9.10.11.12"")]
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);

			// ACT
			var actualContents = VersionFileGenerator.GenerateFileContents("1.2.3.4", "5.6.7.8", "9.10.11.12");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void no_versions_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);
		}

		[Fact]
		public void only_version_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);

			// ACT
			var actualContents = VersionFileGenerator.GenerateFileContents("1.2.3.4");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void version_and_file_version_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
[assembly: AssemblyFileVersion(""5.6.7.8"")]
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);

			// ACT
			var actualContents = VersionFileGenerator.GenerateFileContents("1.2.3.4", "5.6.7.8");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void only_version_and_info_version_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
[assembly: AssemblyInformationalVersion(""9.10.11.12"")]
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);

			// ACT
			var actualContents = VersionFileGenerator.GenerateFileContents("1.2.3.4", null, "9.10.11.12");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}

		[Fact]
		public void only_file_version_and_info_version_supplied()
		{
			// ARRANGE
			var expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyFileVersion(""5.6.7.8"")]
[assembly: AssemblyInformationalVersion(""9.10.11.12"")]
"
			.Replace("\r\n", "\n")
			.Replace("\n", Environment.NewLine);

			// ACT
			var actualContents = VersionFileGenerator.GenerateFileContents(null, "5.6.7.8", "9.10.11.12");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}


	}
}
