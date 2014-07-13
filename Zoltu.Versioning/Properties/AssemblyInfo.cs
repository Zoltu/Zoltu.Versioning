using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription(
@"A NuGet package for automatically versioning builds via the Git repository they are sitting in.

To use, simply tag your releases with ""v#.#"".  The patch version (third number) will calculated by counting the number of commits since the most recent tag matching that format.

When you want to change the major.minor version, just tag a commit with ""v#.#""(e.g., v1.15) and builds of that commit will be numbered v#.#.[commits since tag].0 (e.g., v1.15.0.0 for the build of the commit that is tagged, v1.15.1.0 for the build of the next commit, etc.)")]

[assembly: AssemblyTitle("Automatic Git Versioning")]
[assembly: AssemblyProduct("Zoltu.Versioning")]
[assembly: AssemblyCompany("Zoltu")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
