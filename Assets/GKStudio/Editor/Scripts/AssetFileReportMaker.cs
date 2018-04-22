﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace GKStudio
{
	/**
	 * AssetFileReportMaker.cs
	 * 
	 * ?
	 * ------------------------------------
	 * ?
	 * 
	 * 2018-04-21	GKStudio 	Gustav Knutsson
	 */
	public class AssetFileReportMaker : BaseAssetComparer
	{
		#region Initialize
		[MenuItem(Constants.WindowPath+Constants.WindowFileReportMakerName)]
		static void Init()
		{
			var windowClass = (AssetFileReportMaker)EditorWindow.GetWindow(typeof(AssetFileReportMaker));
			windowClass.Show();
		}

		/// <summary>
		/// When the window opens for the first time
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			// Building
			m_startBuildButtonDelegate = startBuild;
		}
		#endregion

		#region Builder
		/// <summary>
		/// Check if all build options are checked
		/// </summary>
		/// <returns>If we completed all checks or not</returns>
		protected override bool canWeBuildCheck()
		{
			return !string.IsNullOrEmpty(m_firstFolderLocation) && !string.IsNullOrEmpty(m_saveReportFolderLocation);
		}

		/// <summary>
		/// Starts the build.
		/// </summary>
		private static void startBuild(string inS, string inD)
		{
			// Get List of Files
			var firstLocationDict = getAllAssetBundlesFromLocationToDict(m_firstFolderLocation);
			// Save Toggle Data
			saveToggleData();
			// @DEBUG
			foreach(var test in firstLocationDict)
			{
				Debug.LogFormat("Key:{0}, Value:{1}", test.Key, test.Value);
			}
			// Save to file
			prepareWritingFile(firstLocationDict);
		}

		/// <summary>
		/// Prepares the writing file.
		/// </summary>
		/// <param name="inFilesDict">In files dict.</param>
		private static void prepareWritingFile(Dictionary<string, FileMaker> inFilesDict)
		{
			string text = string.Empty;
			string fileFullPath = string.Format("{0}/{1}{2}", m_saveReportFolderLocation, getFileName(Constants.FileNameAssetReporter), Constants.FileNameSuffixType);

			// Add Title Row
			text += FileMaker.GetTitleRow() + Constants.Text.NewLine;

			// Add each row
			foreach(var line in inFilesDict)
			{
				text += line.Value + Constants.Text.NewLine;
			}

			// Write the File
			writeFile(fileFullPath, text);
		}

		/// <summary>
		/// Get File Name that we set, can also be overriden with new key
		/// </summary>
		/// <returns>The full file name.</returns>
		/// <param name="inFileNameBase">Base file name without nothing set.</param>
		protected new static string getFileName(string inFileNameBase)
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
		/// Gets the number stamp, can also be overriden with new key
		/// </summary>
		/// <returns>The number stamp.</returns>
		/// <param name="inFileNameBase">In file name base.</param>
		protected new static string getNumberStamp(string inFileNameBase)
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
		protected new static int getNumberFromPrefs()
		{
			int currentNumber = EditorPrefs.GetInt(Constants.Prefs.FileReporterNumberKey) + 1;
			EditorPrefs.SetInt(Constants.Prefs.FileReporterNumberKey, currentNumber);
			return currentNumber;
		}
		#endregion

		#region UI
		/// <summary>
		/// UI
		/// </summary>
		private void OnGUI()
		{
			guiTitleArea(Constants.Text.ToggleTitle);
			guiCheckBoxArea(Constants.Text.DateStamp, ref m_canUseDateStamp);
			guiCheckBoxArea(Constants.Text.VersionStamp, ref m_canUseVersionStamp);
			guiCheckBoxArea(Constants.Text.NumberStamp, ref m_canUseNumberStamp);

			guiTitleArea(Constants.Text.FolderTitle);
			guiFolderButtonArea(Constants.Text.FirstButton, m_firstFolderLocation, m_firstFolderButtonDelegate);
			guiFolderButtonArea(Constants.Text.SaveReportButton, m_saveReportFolderLocation, m_saveReportFolderButtonDelegate);

			guiTitleArea(Constants.Text.BuildTitle);
			guiStartBuildButton(m_startBuildButtonDelegate);
		}
		#endregion

		#region Jenkins Build
		private static void StartBuildJenkins()
		{
			
		//	startBuild(string.Empty, string.Empty);
		}
		#endregion
	}
}