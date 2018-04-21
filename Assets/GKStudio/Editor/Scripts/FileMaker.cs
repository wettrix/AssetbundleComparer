using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GKStudio
{
	/**
	 * FileMaker.cs
	 * 
	 * ?
	 * ------------------------------------
	 * ?
	 * 
	 * 2018-04-21	GKStudio 	Gustav Knutsson
	 */
	public class FileMaker
	{
		/** Index counter for CSV file */
		private int m_index;
		/** Assetbundle File name */
		private string m_fileName;
		/** Size of AssetBundle */
		private float m_fileSize;
		/** CRC id from file */
		private long m_crc;

		/// <summary>
		/// Constructor class
		/// </summary>
		public FileMaker(int inIndex, FileInfo inFile)
		{
			m_index = inIndex;
			m_fileName = inFile.Name;
			m_fileSize = calculateMegaByteSize(inFile.Length);
			m_crc = 0;
		}

		/// <summary>
		/// Read the CRC file Id from the manifest file and store it
		/// </summary>
		/// <param name="inFile">In file.</param>
		public void SetCrcValueFromReadingFile(FileInfo inFile)
		{
			using(var test = inFile.OpenText())
			{
				bool isFinished = false;
				while(!isFinished)
				{
					var line = test.ReadLine();
					if(!string.IsNullOrEmpty(line))
					{
						if(line.StartsWith(Constants.CrcPrefix))
						{
							try
							{
								m_crc = long.Parse(line.Replace(Constants.CrcPrefix, string.Empty));
								isFinished = true;
							}
							catch(System.Exception ex)
							{
								throw new System.ArgumentException("Error - Failed to parse CRC into long - " + ex.Message);
							}
						}
					}
					// Did not find the File
					else
					{
						test.Close();
						throw new System.ArgumentException("Error - found no CRC");
					}
				}
				test.Close();
			}
		}

		/// <summary>
		/// Calculates the size of the file into MB
		/// </summary>
		/// <returns>The mega byte size.</returns>
		/// <param name="inSize">In file byte size.</param>
		private float calculateMegaByteSize(long inSize)
		{
			const long oneMb = 1000;
			return ((float)inSize / (float)(oneMb * oneMb));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="GKStudio.FileMaker"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="GKStudio.FileMaker"/>.</returns>
		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}", m_index, m_fileName, System.Math.Round(m_fileSize, Constants.DecimalAmountOfMb), m_crc);
		}
	}
}
