//
// Copyright (C) 2010-2013 Kody Brown (kody@bricksoft.com).
// 
// MIT License:
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

// 
// Copyright (C) 2011 Kody Brown (kody.brown@venafi.com)
// Venafi, Inc.
// 

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class GitVersion : Task
{
	public string GitBinPath { get; set; }

	public string CurrentPath { get; set; }

	[Output]
	public string CommitVersion { get; set; }

	[Output]
	public int CommitCount { get; set; }


	public GitVersion()
	{
		// NOTE: The GitBinPath must be set in the .csproj or msbuild file as a Property.
		//       See the http://github.com/kodybrown/shrink project for details.
		GitBinPath = ".";
		CurrentPath = ".";
	}

	public override bool Execute()
	{
		Process p;
		ProcessStartInfo startInfo;
		string val;

		startInfo = new ProcessStartInfo(Path.Combine(GitBinPath, "git.exe"), " log --oneline -1");
		startInfo.UseShellExecute = false;
		startInfo.ErrorDialog = false;
		//oInfo.CreateNoWindow = true;
		startInfo.CreateNoWindow = true;
		startInfo.WorkingDirectory = CurrentPath;
		startInfo.RedirectStandardOutput = true;

		try {
			p = Process.Start(startInfo);
			p.WaitForExit(30000);

			using (StreamReader reader = p.StandardOutput) {
				val = reader.ReadToEnd();
			}

			if (val != null) {
				CommitVersion = val.Split(' ')[0];
			} else {
				CommitVersion = "not found";
			}

			Console.WriteLine("CommitVersion === \"" + CommitVersion + "\"");
		} catch (Exception ex) {
			Console.WriteLine(ex.Message);
			return false;
		}

		startInfo = new ProcessStartInfo(Path.Combine(GitBinPath, "git.exe"), "log --oneline");
		startInfo.UseShellExecute = false;
		startInfo.ErrorDialog = false;
		//oInfo.CreateNoWindow = true;
		startInfo.CreateNoWindow = true;
		startInfo.WorkingDirectory = CurrentPath;
		startInfo.RedirectStandardOutput = true;

		try {
			int tempValue;

			p = Process.Start(startInfo);
			p.WaitForExit(30000);

			tempValue = 0;

			using (StreamReader reader = p.StandardOutput) {
				while (!reader.EndOfStream) {
					reader.ReadLine();
					tempValue++;
				}
			}

			CommitCount = tempValue;
			//Console.WriteLine("CommitCount === \"" + CommitCount + "\"");

			return true;
		} catch (Exception ex) {
			Console.WriteLine(ex.Message);
			return false;
		}
	}
}
