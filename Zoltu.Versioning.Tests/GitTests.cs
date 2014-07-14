using Xunit;

namespace Zoltu.Versioning.Tests
{
	public class GitTests
	{
		[Fact]
		public void when_foo_then_bar()
		{
			// ARRANGE
			const string expectedContents =
@"// This is a generated file.  Do not commit it to version control and do not modify it.
using System.Reflection;
[assembly: AssemblyVersion(""1.2.3.4"")]
[assembly: AssemblyFileVersion(""1.2.3.4"")]
";

			// ACT
			var actualContents = GitVersion.GenerateVersionFileContents("1.2.3.4");

			// ASSERT
			Assert.Equal(expectedContents, actualContents);
		}
	}
}
