using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Zoltu.Versioning
{
	public static class VersionFileGenerator
	{
		public static String GenerateFileContents(String version, String fileVersion = null, String infoVersion = null)
		{
			Contract.Requires(version != null);
			Contract.Ensures(Contract.Result<String>() != null);

			if (fileVersion == null)
				fileVersion = version;
			if (infoVersion == null)
				infoVersion = version;

			return new StringBuilder()
				.AppendLine(@"// This is a generated file.  Do not commit it to version control and do not modify it.")
				.AppendLine(@"using System.Reflection;")
				.AppendLine($"[assembly: AssemblyVersion(\"{version}\")]")
				.AppendLine($"[assembly: AssemblyFileVersion(\"{fileVersion}\")]")
				.AppendLine($"[assembly: AssemblyInformationalVersion(\"{infoVersion}\")]")
				.ToString();
		}
	}
}
