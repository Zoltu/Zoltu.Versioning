using Xunit;

namespace Zoltu.Versioning.Tests
{
	public class GitTests
	{
		[Fact]
		public void when_version_supplied()
		{
			// ARRANGE
			const string expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
[assembly: AssemblyFileVersion(""5.6.7.8"")]
[assembly: AssemblyInformationalVersion(""9.10.11.12"")]
";
			var version = new Version(1, 2, 3, 4);
			var fileVersion = new Version(5, 6, 7, 8);
			var infoVersion = new Version(9, 10, 11, 12);

			// ACT
			var actualContents = GitVersion.GenerateVersionFileContents(version, fileVersion, infoVersion);

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}
	}
}
