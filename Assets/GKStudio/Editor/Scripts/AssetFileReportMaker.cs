using System.Collections;
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

			foreach(var test in firstLocationDict)
			{
				Debug.LogFormat("Key:{0}, Value:{1}", test.Key, test.Value);
			}
			// Save to file
			// @TODO
		}


		#endregion

		#region UI
		/// <summary>
		/// UI
		/// </summary>
		void OnGUI()
		{
			guiFolderButtonArea(Constants.Text.FirstButton, m_firstFolderLocation, m_firstFolderButtonDelegate);
			guiFolderButtonArea(Constants.Text.SaveReportButton, m_saveReportFolderLocation, m_saveReportFolderButtonDelegate);
			guiStartBuildButton(m_startBuildButtonDelegate);
		}
		#endregion


	}
}