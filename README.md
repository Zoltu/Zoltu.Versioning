# Zoltu.Version

NuGet package for automatically versioning a project with git.

## Usage

 * Install the NuGet package (https://www.nuget.org/packages/Zoltu.Versioning/).
 * Remove `AssemblyVersion`, `AssemblyFileVersion` and `AssemblyInformationalVersion` attributes from your AssemblyInfo.cs (if you have them).
 * When you want to increase the major or minor version numbers, tag the commit that should bump the version with v#.#
  * Example Tag: v3.5
  * Note: If you are using a build server that automatically builds on commit, it is recommended that you tag *before* pushing to the remote master.  This is because your build server will kick off without that tag and therefore the build that is generated will not have the newly tagged version.

## Versioning

The versioning system is simple, and designed to follow semantic versioning for CI/CD projects more or less.  When a build is kicked off, a VersionAssemblyInfo.cs file is generated (no need to add it to your project, it is compiled in automatically).  The VersionAssemblyInfo.cs file contains `AssemblyVersion`, `AssemblyFileVersion` and `AssemblyInformationalVersion` assembly attributes.

The first step to figuring out the version is getting the git repository.  This is done by traversing the directory tree until a valid .git folder is found.  If none is found then the version is 0.0.0.0.

The second step is to walk the commit history from HEAD to the first commit, looking for a tag with a name that matches v#.# (the actual regex is `^v(\d+).(\d+)$`). If such a tag is found, then the number of commits traversed are counted to generate the patch version and the major and minor versions are parsed from the tag.

## Example

You create a new project.  You make a couple commits.  You tag the second commit on the repo v1.0.  When you build HEAD at this point, your assembly will be versioned 1.0.0.0.  If you then commit two more times and build, your assembly will be versioned 1.0.2.  You then commit again and tag the commit v3.5 and build, your assembly will be 3.5.0.0.  If you then commit 10 more times and build your assembly will be versioned 3.5.10.0.

Note: The build version never changes because in the .NET world it is ignored.

### Skipping revision and build parts

It is possible to instruct the versioning system to output only a version consiting of only major and minor. This can be done with each of the three version attributes independently by setting relevant variables in the .csproj file.

```
<GitVersionOnlyMajorAndMinorInAssemblyVersion />
<GitVersionOnlyMajorAndMinorInAssemblyFileVersion />
<GitVersionOnlyMajorAndMinorInAssemblyInformationalVersion />
```

When the first is set to `true` in the above example the output attributes will be as shown below.

``` xml
<PropertyGroup>
    <GitVersionOnlyMajorAndMinorInAssemblyVersion>
        true
    </GitVersionOnlyMajorAndMinorInAssemblyVersion>
</PropertyGroup>
```

``` c#
[assembly: AssemblyVersion("3.5.0.0")]
[assembly: AssemblyFileVersion("3.5.10.0")]
[assembly: AssemblyInformationalVersion("3.5.10.0")]
```

### NuGet-compatible prerelease versions

As of version 1.1.23 it is possible to extend the tag with a suffix, which will be appended to the `AssemblyInformationalVersion` attribute. For example a tag `v3.5-RC05` the informational version will be 3.5.0.0-RC05. When you push 10 commits it will be 3.5.10.0-RC05.

This is useful when producing NuGet prerelase packages from project or when the nuspec is updated from assembly metadata.

### NCrunch Integration

NCrunch builds will start failing after adding this project to your solution due to being unable to find LibGit2Sharp libraries `System.IO.FileNotFoundException: Could not load file or assembly 'LibGit2Sharp, Version=0.18.1.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified.`
To work around this, add the following to your .ncrunchproject file:

``` xml
<AdditionalFilesToInclude>..\packages\Zoltu.Versioning.*\build\**.*</AdditionalFilesToInclude>
```
