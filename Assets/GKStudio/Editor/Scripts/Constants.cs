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
			public const string NewLine = "\n";

			#region Titles
			/**  */
			public const string ToggleTitle = "File Name Options";
			/**  */
			public const string FolderTitle = "Folder Options";
			/**  */
			public const string BuildTitle = "Build";
			#endregion

			/** Builder Date Stamp */
			public const string DateStamp =  "Add Date Stamp to File Name";
			/** Builder Version Stamp */
			public const string VersionStamp = "Add Version Stamp to File Name";
			/** Builder number Stamp */
			public const string NumberStamp = "Add Number Stamp to File Name";

			/** First Open Folder */
			public static readonly ButtonTextStruct FirstButton = new ButtonTextStruct
			{
				MainText = "Select the AssetBundle folder path",
				ButtonText = "Select AssetBundle Folder",
				FolderMainText = "Select Folder"
			};
			/** Second Open Folder */
			public static readonly ButtonTextStruct SecondButton = new ButtonTextStruct
			{
				MainText = "Select the other AssetBundle folder path to compare with",
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

			#region File writer
			/** Top Row Index */
			public const string TopRowIndex = "Index";
			/** Top Row Assetbundle Name */
			public const string TopRowAssetbundleName = "Assetbundle Name";
			/** Top Row MegaBytes */
			public const string TopRowMegaByte = "MB Size";
			/** Top Row CRC ID */
			public const string TopRowCrc = "CRC ID";
			#endregion
		}
		#endregion

		#region Player Prefs
		public class Prefs
		{
			/** Basic helper prefix */
			private const string prefix = "GKStudio_";
			/** Save for First folder */
			public const string FirstFolderPathKey = prefix + "firstFolder";
			/** Save for Second folder */
			public const string SecondFolderPathKey = prefix + "secondFolder";
			/** Save for CSV destination folder */
			public const string SaveReportFolderPathKey = prefix + "saveReportFolder";
			/** File Version for AssetBundle File Reporter */
			public const string FileReporterNumberKey = prefix + "fileReportNumber";
			/** File Version for AssetBundle Comparer Reporter */
			public const string CompareReporterNumberKey = prefix + "compareReportNumber";
			/** Builder Date Stamp */
			public const string DateStampKey = prefix + "dateStamp";
			/** Builder Version Stamp */
			public const string VersionStampKey = prefix + "versionStamp";
			/** Builder number Stamp */
			public const string NumberStampKey = prefix + "numberStamp";
		}
		#endregion

		#region Strings
		/** Meta Suffix */
		public const string MetaSuffix = ".meta";
		/** Manifest Suffix */
		public const string ManifestSuffix = ".manifest";
		/** CRC Prefix*/
		public const string CrcPrefix = "CRC: ";
		#endregion

		#region File writer
		/**  */
		public const string FileNameSuffixType = ".csv";

		#region Asset File Report
		/**  */
		public const string FileNameAssetReporter = "AssetBundleFileReport";
		#endregion

		#region Asset File Comparer Report
		/**  */
		public const string FileNameComparerReporter = "AssetBundleComparerReport";
		#endregion
		#endregion

		#region Other Values
		/** Number of Decimals in MB suffix */
		public const int DecimalAmountOfMb = 2;
		#endregion
	}
}