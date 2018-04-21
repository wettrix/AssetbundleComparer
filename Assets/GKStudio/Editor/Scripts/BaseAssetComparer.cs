using System;
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
		protected string m_firstFolderLocation = string.Empty;
		/** First Folder Button Press */
		protected Action<string> m_firstFolderButtonDelegate;

		// Save CSV Folder
		/** Save CSV Folder Path */
		protected string m_saveReportFolderLocation = string.Empty;
		/** Save CSV Folder Button Press */
		protected Action<string> m_saveReportFolderButtonDelegate;
		#endregion

		#region Initialize
		/// <summary>
		/// When the window opens for the first time
		/// </summary>
		virtual protected void OnEnable()
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
		virtual protected bool canWeBuildCheck()
		{
			return true;
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
		protected void guiStartBuildButton()
		{
			// If all checks are complete
			if(canWeBuildCheck())
			{
				GUI.skin.button.alignment = TextAnchor.MiddleCenter;
				GUI.backgroundColor = Color.green;
				if(GUILayout.Button(Constants.Text.StartBuildingLabel, GUILayout.Width(Constants.ButtonFolderWidth)))
				{
					
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