GitVersion
==========

GitVersion.dll is a basic MSBuild extension for versioning assemblies, based on the Git revision count. See the source code for more details about that.

The Git.exe binary is required for this to work. And, the MSBuild Community Tasks is also required for some of this to work.

In order to use this extension, add the following to your .csproj files and reload your project:

    <PropertyGroup Label="Versioning">
      <VersioningMajor>0</VersioningMajor>
      <VersioningMinor>9</VersioningMinor>
      <CommitVersion>N/A</CommitVersion>
    </PropertyGroup>
    <PropertyGroup>
      <GitBinPath>C:\YourPath\Git\bin</GitBinPath>
    </PropertyGroup>
    <UsingTask TaskName="GitVersion" AssemblyFile="C:\YourPath\GitVersion.dll" />
    <Target Name="BeforeBuild">
      <GitVersion GitBinPath="$(GitBinPath)" CurrentPath="$(MSBuildProjectDirectory)">
        <Output TaskParameter="CommitVersion" PropertyName="CommitVersion" />
        <Output TaskParameter="CommitCount" PropertyName="CommitCount" />
      </GitVersion>
      <Time Format="yyMMdd">
        <Output TaskParameter="FormattedTime" PropertyName="BuildDate" />
        <Output TaskParameter="Year" PropertyName="Year" />
      </Time>
      <AssemblyInfo 
        CodeLanguage="CS" 
        OutputFile="Properties\AssemblyInfo.cs" 
        AssemblyTitle="appName ver:$(VersioningMajor).$(VersioningMinor).$(BuildDate.Substring(1)).$(CommitCount)" 
        AssemblyDescription="appName" 
        AssemblyCompany="Company Name" 
        AssemblyProduct="appName" 
        AssemblyCopyright="Copyright (C) 2003-$(Year) Author's Name." 
        ComVisible="false" 
        CLSCompliant="false" 
        Guid="11111111-2222-3333-4444-555555555555" 
        AssemblyVersion="$(VersioningMajor).$(VersioningMinor).$(BuildDate.Substring(1)).$(CommitCount)" 
        AssemblyFileVersion="$(VersioningMajor).$(VersioningMinor).$(BuildDate.Substring(1)).$(CommitCount)" />
      <Message Text="$(VersioningMajor).$(VersioningMinor).$(BuildDate.Substring(1)).$(CommitCount)" />
    </Target>

Please note that this will overwrite your AssemblyInfo.cs file!

