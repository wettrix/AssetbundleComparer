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
		/** Add a Date Stamp after Main Name */
		protected static bool m_canUseDateStamp;
		/** Add a Build Version stamp if provided after Main Name */
		protected static bool m_canUseVersionStamp;
		/** Add a locally stored Number Index Stamp after Main Name */
		protected static bool m_canUseNumberStamp;
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
			// Building
			m_canUseDateStamp = EditorPrefs.GetBool(Constants.Prefs.DateStampKey);
			m_canUseVersionStamp = EditorPrefs.GetBool(Constants.Prefs.VersionStampKey);
			m_canUseNumberStamp = EditorPrefs.GetBool(Constants.Prefs.NumberStampKey);
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

		/** Curernt locally stored Number stamp */

		/// <summary>
		/// Get File Name that we set, can also be overriden with new key
		/// </summary>
		/// <returns>The full file name.</returns>
		/// <param name="inFileNameBase">Base file name without nothing set.</param>
		protected static string getFileName(string inFileNameBase)
		{
			// Add Date
			inFileNameBase = getDateStamp(inFileNameBase);
			// Add Version
			inFileNameBase = getVersionStamp(inFileNameBase);
			// Add Number
			inFileNameBase = getNumberStamp(inFileNameBase);

			return inFileNameBase;
		}

		/// <summary>
		/// Gets the date stamp, can also be overriden with new key
		/// </summary>
		/// <returns>The date stamp.</returns>
		/// <param name="inFileNameBase">In file name base.</param>
		protected static string getDateStamp(string inFileNameBase)
		{
			if(m_canUseDateStamp)
			{
				DateTime currentDate = DateTime.Now;
				inFileNameBase += string.Format("_{0}-{1}-{2}", currentDate.Year, currentDate.Month, currentDate.Day);
			}
			return inFileNameBase;
		}

		/// <summary>
		/// Gets the version stamp, can also be overriden with new key
		/// </summary>
		/// <returns>The version stamp.</returns>
		/// <param name="inFileNameBase">In file name base.</param>
		protected static string getVersionStamp(string inFileNameBase)
		{
			if(m_canUseVersionStamp)
			{
				inFileNameBase += string.Format("_{0}", Application.version);
			}
			return inFileNameBase;
		}

		/// <summary>
		/// Gets the number stamp, can also be overriden with new key
		/// </summary>
		/// <returns>The number stamp.</returns>
		/// <param name="inFileNameBase">In file name base.</param>
		protected static string getNumberStamp(string inFileNameBase)
		{
			if(m_canUseNumberStamp)
			{
				inFileNameBase += string.Format("_{0}", getNumberFromPrefs());
			}
			return inFileNameBase;
		}

		/// <summary>
		/// Get the current locally stored Number stamp, can also be overriden with new key
		/// </summary>
		/// <returns>Current number</returns>
		protected static int getNumberFromPrefs()
		{
			return 0;
		}

		/// <summary>
		/// Writes the file.
		/// </summary>
		/// <param name="inFullFilePath">In full file path.</param>
		/// <param name="inTextContents">In text contents.</param>
		protected static void writeFile(string inFullFilePath, string inTextContents)
		{
			// Write the File, also does overwrite if needed
			File.WriteAllText(inFullFilePath, inTextContents);
		}

		/// <summary>
		/// Saves the toggle data.
		/// </summary>
		protected static void saveToggleData()
		{
			EditorPrefs.SetBool(Constants.Prefs.DateStampKey, m_canUseDateStamp);
			EditorPrefs.SetBool(Constants.Prefs.VersionStampKey, m_canUseVersionStamp);
			EditorPrefs.SetBool(Constants.Prefs.NumberStampKey, m_canUseNumberStamp);
		}
		#endregion

		#region UI
		/// <summary>
		/// Create a Title Label
		/// </summary>
		/// <param name="inStyle">Text Label</param>
		protected void guiTitleArea(string inStyle)
		{
			EditorGUILayout.LabelField(inStyle, EditorStyles.boldLabel);
		}

		/// <summary>
		/// Creates a Checkbox Toggle area
		/// </summary>
		/// <param name="inStyle">Specific text we will use for the toggle.</param>
		/// <param name="inValue">What boolean value we add. Also return it.</param>
		protected void guiCheckBoxArea(string inStyle, ref bool inValue)
		{
			inValue = EditorGUILayout.ToggleLeft(inStyle, inValue);
		}

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
				string folderResult = EditorUtility.OpenFolderPanel(inStyle.FolderMainText, inResult, string.Empty);
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