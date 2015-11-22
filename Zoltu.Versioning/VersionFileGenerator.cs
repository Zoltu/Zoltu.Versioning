using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Zoltu.Versioning
{
	public static class VersionFileGenerator
	{
		public static String GenerateFileContents(String version = null, String fileVersion = null, String infoVersion = null)
		{
			Contract.Ensures(Contract.Result<String>() != null);

			var stringBuilder = new StringBuilder()
				.AppendLine(@"// This is a generated file.  Do not commit it to version control and do not modify it.")
				.AppendLine(@"using System.Reflection;");

			if (version != null)
				stringBuilder.AppendLine($"[assembly: AssemblyVersion(\"{version}\")]");
			if (fileVersion != null)
				stringBuilder.AppendLine($"[assembly: AssemblyFileVersion(\"{fileVersion}\")]");
			if (infoVersion != null)
				stringBuilder.AppendLine($"[assembly: AssemblyInformationalVersion(\"{infoVersion}\")]");

			return stringBuilder.ToString();
		}
	}
}
