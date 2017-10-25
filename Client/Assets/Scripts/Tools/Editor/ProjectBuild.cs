using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

class ProjectBuild : Editor{

	//在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}

	//[@MenuItem("BoLong/Build  BuildProject/Android")]
	static void BuildForAndroid()
	{

		//在这里分析shell传入的参数， 还记得上面我们说的哪个 project-$1 这个参数吗？
		//这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
		//这个字符串就是 91 了。。
		string projectName = "EOP";
		foreach(string arg in System.Environment.GetCommandLineArgs()) {
			if(arg.StartsWith("project"))
			{
				projectName = arg.Split("-"[0])[1];
				break;
			}
		}
			
		Debug.Log(BuildPipeline.BuildPlayer(GetBuildScenes(), projectName + ".apk", BuildTarget.Android, BuildOptions.None));
	}
}