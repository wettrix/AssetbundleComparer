using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
		#endregion

		#region Builder
		/// <summary>
		/// Check if all build options are checked
		/// </summary>
		/// <returns>If we completed all checks or not</returns>
		override protected bool canWeBuildCheck()
		{
			return !string.IsNullOrEmpty(m_firstFolderLocation) && !string.IsNullOrEmpty(m_saveReportFolderLocation);
		}

		private static void startBuild()
		{

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
			guiStartBuildButton();
		}
		#endregion


	}
}