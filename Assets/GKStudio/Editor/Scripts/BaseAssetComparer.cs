using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace GKStudio
{
	/**
	 * BaseAssetComparer.cs
	 * 
	 * ?
	 * ------------------------------------
	 * ?
	 * 
	 * 2018-04-21	GKStudio 	Gustav Knutsson
	 */
	public class BaseAssetComparer : EditorWindow
	{
		#region Variables
		// First Folder
		/** First Folder Path */
		protected static string m_firstFolderLocation = string.Empty;
		/** First Folder Button Press */
		protected Action<string> m_firstFolderButtonDelegate;

		// Save CSV Folder
		/** Save CSV Folder Path */
		protected static string m_saveReportFolderLocation = string.Empty;
		/** Save CSV Folder Button Press */
		protected Action<string> m_saveReportFolderButtonDelegate;

		// Building
		/** Start Building Button Press */
		protected Action<string, string> m_startBuildButtonDelegate;
		#endregion

		#region Initialize
		/// <summary>
		/// When the window opens for the first time
		/// </summary>
		protected virtual void OnEnable()
		{
			// First Folder
			m_firstFolderButtonDelegate = onPressSelectFirstFolderLocation;
			m_firstFolderLocation = EditorPrefs.GetString(Constants.Prefs.FirstFolderPathKey);
			// Save CSV Folder
			m_saveReportFolderButtonDelegate = onPressSaveReportFolderLocation;
			m_saveReportFolderLocation = EditorPrefs.GetString(Constants.Prefs.SaveReportFolderPathKey);
		}
		#endregion

		#region Builder
		/// <summary>
		/// Check if all build options are checked
		/// </summary>
		/// <returns>If we completed all checks or not</returns>
		protected virtual bool canWeBuildCheck()
		{
			return true;
		}
		/// <summary>
		/// Store all Assetbundles in a Dictionary and return it
		/// </summary>
		/// <returns>All Assetbundle data</returns>
		/// <param name="inPath">In path.</param>
		protected static Dictionary<string, FileMaker> getAllAssetBundlesFromLocationToDict(string inPath)
		{
			// Get assetbundle and manifest files
			var fileInfoClass = new DirectoryInfo(inPath);
			var assetBundleList = new List<FileInfo>();
			var manifestList = new List<FileInfo>();

			// Sort files
			foreach(var file in fileInfoClass.GetFiles())
			{
				if(!file.Name.EndsWith(Constants.MetaSuffix))
				{
					// If Manifest file
					if(file.Name.EndsWith(Constants.ManifestSuffix))
						manifestList.Add(file);
					// If Other Assetbundle File
					else
						assetBundleList.Add(file);
				}
			}

			// Error check
			if(assetBundleList.Count == 0 || manifestList.Count == 0)
				throw new ArgumentException("Error - no Files found!");

			// make FileMaker
			var fileMakerDict = new Dictionary<string, FileMaker>();
			createFileMaker(assetBundleList, fileMakerDict);
			createFileMaker(manifestList, fileMakerDict);

			return fileMakerDict;
		}

		/// <summary>
		/// Creates each Assetbundle class
		/// </summary>
		/// <param name="inSearchList">Files List.</param>
		/// <param name="fileMakerDict">File maker dict.</param>
		private static void createFileMaker(List<FileInfo> inSearchList, Dictionary<string, FileMaker> fileMakerDict)
		{
			foreach(var file in inSearchList)
			{
				// Manifest file search
				if(file.Name.Contains(Constants.ManifestSuffix))
				{
					var fileExtensionless = Path.GetFileNameWithoutExtension(file.Name);
					if(fileMakerDict.ContainsKey(fileExtensionless))
					{
						fileMakerDict[fileExtensionless].SetCrcValueFromReadingFile(file);
					}
				}
				// If we are Assetbundle, then create a
				else
				{
					fileMakerDict.Add(file.Name, new FileMaker(fileMakerDict.Count, file));
				}
			}
		}
		#endregion

		#region UI
		/// <summary>
		/// Creates a Folder button selector area
		/// </summary>
		/// <param name="inStyle">Specific style we use</param>
		/// <param name="inButtonPress">Button Return press</param>
		protected void guiFolderButtonArea(Constants.ButtonTextStruct inStyle, string inResult, Action<string> inButtonPress)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUILayout.Label(inStyle.MainText);
			GUILayout.BeginHorizontal();
			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			if(GUILayout.Button(inStyle.ButtonText, GUILayout.Width(Constants.ButtonFolderWidth)))
			{
				string folderResult = EditorUtility.OpenFolderPanel(inStyle.FolderMainText, string.Empty, string.Empty);
				if(inButtonPress != null)
					inButtonPress(folderResult);
			}
			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUILayout.Box(inResult, GUILayout.ExpandWidth(true));
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// creates a final button for starting the main script
		/// </summary>
		protected void guiStartBuildButton(Action<string, string> inButtonPress)
		{
			// If all checks are complete
			if(canWeBuildCheck())
			{
				GUI.skin.button.alignment = TextAnchor.MiddleCenter;
				GUI.backgroundColor = Color.green;
				if(GUILayout.Button(Constants.Text.StartBuildingLabel, GUILayout.Width(Constants.ButtonFolderWidth)))
				{
					if(inButtonPress != null)
						inButtonPress(m_firstFolderLocation, string.Empty);
				}
			}
			// If a check is missing
			else
			{
				GUI.backgroundColor = Color.gray;
				GUI.skin.box.alignment = TextAnchor.MiddleCenter;
				GUILayout.Box(Constants.Text.CantBuildLabel, GUILayout.Width(Constants.ButtonFolderWidth));
			}
		}
		#endregion

		#region ButtonPresses
		/// <summary>
		/// When Pressing the Open folder in guiFolderButtonArea function we recieve a callback with path data
		/// </summary>
		/// <param name="inResult">Path result</param>
		private void onPressSelectFirstFolderLocation(string inResult)
		{
			m_firstFolderLocation = inResult;
			EditorPrefs.SetString(Constants.Prefs.FirstFolderPathKey, m_firstFolderLocation);
		}

		/// <summary>
		/// When Pressing the Open folder in guiFolderButtonArea function we recieve a callback with path data
		/// </summary>
		/// <param name="inResult">Path result</param>
		private void onPressSaveReportFolderLocation(string inResult)
		{
			m_saveReportFolderLocation = inResult;
			EditorPrefs.SetString(Constants.Prefs.SaveReportFolderPathKey, m_saveReportFolderLocation);
		}
		#endregion
	}
}