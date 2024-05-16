using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Imagine.WebAR.Editor
{
	public class WorldWebARMenu
	{
		[MenuItem("Assets/Imagine WebAR/Create/WorldTracker", false, 1100)]
		public static void CreateWorldTracker()
		{
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagine/WorldTracker/Prefabs/WorldTracker.prefab");
			GameObject gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
			Selection.activeGameObject = gameObject;
			gameObject.name = "WorldTracker";
		}

		[MenuItem("Assets/Imagine WebAR/Create/AR Camera", false, 1120)]
		public static void CreateARCamera()
		{
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagine/WorldTracker/Prefabs/ARCamera.prefab");
			GameObject gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
			Selection.activeGameObject = gameObject;
			gameObject.name = "ARCamera";
		}

		[MenuItem("Assets/Imagine WebAR/Update Plugin to URP", false, 1200)]
		public static void SetURP()
		{
			if (EditorUtility.DisplayDialog(
				"Update Imagine WebAR Plugin to URP",
				"Please make sure that the Universal RP package is already installed before doing this step.",
				"Proceed",
				"Cancel"))
			{
				string[] files = Directory.GetFiles(Application.dataPath + "/Imagine/WorldTracker/Demos/Materials", "*.mat", SearchOption.TopDirectoryOnly);
				foreach (var file in files)
				{
					var path = file.Replace(Application.dataPath, "Assets");
					var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
					if (mat.shader.name == "Standard")
					{
						mat.shader = Shader.Find("Universal Render Pipeline/Lit");
					}
					else if (mat.shader.name == "Imagine/ARShadow")
                    {
						mat.shader = Shader.Find("Imagine/ARShadowURP");
					}
				}

				AddDefineSymbol("IMAGINE_URP");
				EditorUtility.DisplayDialog("Completed", "Imagine WebAR Plugin is now set to URP. \n\nSome URP features such as HDR and Post-Processing may be partially/fully unsupported.", "Ok");
			}
		}


		[MenuItem("Assets/Imagine WebAR/Roll-back Plugin to Built-In RP", false, 1201)]
		public static void SetBuiltInRP ()
		{
			if (EditorUtility.DisplayDialog(
				"Roll-back Imagine WebAR Plugin to Built-In RP",
				"Plese confirm.",
				"Proceed",
				"Cancel"))
			{
				string[] files = Directory.GetFiles(Application.dataPath + "/Imagine/WorldTracker/Demos/Materials", "*.mat", SearchOption.TopDirectoryOnly);
				foreach (var file in files)
				{
					var path = file.Replace(Application.dataPath, "Assets");
					var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
					if (mat.shader.name == "Universal Render Pipeline/Lit" || mat.shader.name == "Hidden/InternalErrorShader")
					{
						mat.shader = Shader.Find("Standard");
					}
					else if (mat.shader.name == "Imagine/ARShadowURP")
					{
						mat.shader = Shader.Find("Imagine/ARShadow");
					}
				}

				RemoveDefineSymbol("IMAGINE_URP");

				EditorUtility.DisplayDialog("Completed", "Imagine WebAR Plugin is now set to Built-In RP. Some edited materials may still require manual shader change","Ok");

			}
		}

		public static void AddDefineSymbol(string symbol)
		{
			string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			List<string> allDefines = definesString.Split(';').ToList();
			if (!allDefines.Contains(symbol))
				allDefines.Add(symbol);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(
				 EditorUserBuildSettings.selectedBuildTargetGroup,
				 string.Join(";", allDefines.ToArray()));
			AssetDatabase.RefreshSettings();
		}

		public static void RemoveDefineSymbol(string symbol)
		{
			string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			List<string> allDefines = definesString.Split(';').ToList();
			allDefines.RemoveAll(s => s == symbol);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(
				 EditorUserBuildSettings.selectedBuildTargetGroup,
				 string.Join(";", allDefines.ToArray()));
			AssetDatabase.RefreshSettings();

		}
	}
}

