Zoltu.Version
=============

NuGet package for automatically versioning a project with git.

Usage
=====
 * Install the NuGet package (https://www.nuget.org/packages/Zoltu.Versioning/).
 * Remove `AssemblyVersion` and `AssemblyFileVersion` attributes from your AssemblyInfo.cs (if you have them).
 * When you want to increase the major or minor version numbers, tag the commit that should bump the version with v#.#
  * Example Tag: v3.5
  * Note: If you are using a build server that automatically builds on commit, it is recommended that you tag *before* pushing to the remote master.  This is because your build server will kick off without that tag and therefore the build that is generated will not have the newly tagged version.

Versioning
==========
The versioning system is simple, and designed to follow semantic versioning for CI/CD projects more or less.  When a build is kicked off, a VersionAssemblyInfo.cs file is generated (no need to add it to your project, it is compiled in automatically).  The VersionAssemblyInfo.cs file contains `AssemblyVersion` and `AssemblyFileVersion` assembly attributes.

The first step to figuring out the version is getting the git repository.  This is done by traversing the directory tree until a valid .git folder is found.  If none is found then the version is 0.0.0.0.

The second step is to walk the commit history from HEAD to the first commit, looking for a tag with a name that matches v#.# (the actual regex is `^v(\d+)\.(\d+)$`).  If such a tag is found, then the number of commits traversed are counted to generate the patch version and the major and minor versions are parsed from the tag.

Example
=======
You create a new project.  You make a couple commits.  You tag the second commit on the repo v1.0.  When you build HEAD at this point, your assembly will be versioned 1.0.0.0.  If you then commit two more times and build, your assembly will be versioned 1.0.2.  You then commit again and tag the commit v3.5 and build, your assembly will be 3.5.0.0.  If you then commit 10 more times and build your assembly will be versione 3.5.10.0.

Note: The build version never changes because in the .NET world it is ignored.
