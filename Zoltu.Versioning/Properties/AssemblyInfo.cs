using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription(
@"A NuGet package for automatically versioning builds via the Git repository they are sitting in.

Usage: Tag your releases with ""v#.#"".

Additional Details: https://github.com/Zoltu/Zoltu.Versioning/blob/master/README.md")]

[assembly: AssemblyTitle("Automatic Git Versioning")]
[assembly: AssemblyProduct("Zoltu.Versioning")]
[assembly: AssemblyCompany("Zoltu")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyCulture("")]
