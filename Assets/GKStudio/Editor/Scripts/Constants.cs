namespace GKStudio
{
	/**
	 * Constants.cs
	 * 
	 * ?
	 * ------------------------------------
	 * ?
	 * 
	 * 2018-04-21	GKStudio 	Gustav Knutsson
	 */
	public class Constants
	{
		/** Basic Text helper struct for Folder button GUI */
		public struct ButtonTextStruct
		{
			/** Main explenation Text */
			public string MainText;
			/** Button explanation text */
			public string ButtonText;
			/** Opening Folder explanation text */
			public string FolderMainText;
		};

		#region Main Data
		/** Version of the package */
		public const string Version = "1.0";
		#endregion

		#region Window data
		/** Top Window Path */
		public const string WindowPath = "GKStudio/";
		/** Name of Window for the File Reporter */
		public const string WindowFileReportMakerName = "Assetbundle File Reporter";
		/** Name of Window for the Compare Reporter */
		public const string WindowComparerRepportMaker = "Assetbundle Compare Reporter";
		#endregion

		#region Layout Style
		/** Button width */
		public const float ButtonFolderWidth = 200f;
		#endregion

		#region Button Texts
		public class Text
		{
			/** First Open Folder */
			public static readonly ButtonTextStruct FirstButton = new ButtonTextStruct
			{
				MainText = "Select the AssetBundle folder path",
				ButtonText = "Select AssetBundle Folder",
				FolderMainText = "Select Folder"
			};
			/** Save Report Open Folder */
			public static readonly ButtonTextStruct SaveReportButton = new ButtonTextStruct
			{
				MainText = "Select where you want to save your Report",
				ButtonText = "Select Report Folder",
				FolderMainText = "Select Folder"
			};
			/** Text for start bulding */
			public const string StartBuildingLabel = "Start Building";
			/** Text for when you can't build */
			public const string CantBuildLabel = "Can't Build yet";
		}
		#endregion

		#region Player Prefs
		public class Prefs
		{
			/** Basic helper prefix */
			private const string prefix = "GKStudio_";
			/** Save for First folder */
			public const string FirstFolderPathKey = prefix + "firstFolder";
			/** Save for CSV destination folder */
			public const string SaveReportFolderPathKey = prefix + "saveReportFolder";
		}
		#endregion
	}
}