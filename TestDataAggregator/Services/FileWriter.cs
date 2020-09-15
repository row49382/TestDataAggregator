using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestInformationAggregator.Services
{
	public static class FileWriter
	{
		/// <summary>
		/// Writes the file contents to the specified location
		/// </summary>
		/// <param name="filePath"> The file path to write to</param>
		/// <param name="fileName"> THe file name to write the file as </param>
		/// <param name="fileContents"> The contents of the file</param>
		public static void Write(string filePath, string fileName, string fileContents)
		{
			File.WriteAllText(Path.Combine(filePath, fileName), fileContents);
		}
	}
}
