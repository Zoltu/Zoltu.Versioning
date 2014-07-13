using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Zoltu.Versioning
{
	public class Git : Task
	{
		[Required]
		public String OutputFilePath { get; set; }

		public override Boolean Execute()
		{
			Contract.Assume(Log != null);

			var version = GetVersionFromGit();
			var fileContents = GenerateVersionFileContents(version);
			File.WriteAllText(OutputFilePath, fileContents);

			return true;
		}

		public static String GetVersionFromGit()
		{
			Contract.Ensures(Contract.Result<String>() != null);

			return "1.2.3.4";
		}

		public static String GenerateVersionFileContents(String version)
		{
			Contract.Requires(version != null);
			Contract.Ensures(Contract.Result<String>() != null);

			var builder = new StringBuilder();
			builder.AppendLine(@"// This is a generated file.  Do not commit it to version control and do not modify it.");
			builder.AppendLine(@"using System.Reflection;");
			builder.AppendFormat(@"[assembly: AssemblyVersion(""{0}"")]{1}", version, Environment.NewLine);
			builder.AppendFormat(@"[assembly: AssemblyFileVersion(""{0}"")]{1}", version, Environment.NewLine);
			return builder.ToString();
		}
	}
}
