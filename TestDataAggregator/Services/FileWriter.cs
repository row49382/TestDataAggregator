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
		/// <param name="fileName"> The file name to write the file as </param>
		/// <param name="fileContents"> The contents of the file</param>
		public static void Write(string filePath, string fileName, string fileContents)
		{
			Requires.NotNull(filePath);
			Requires.NotNull(fileName);

			VerifyDirectoryExists(filePath);

			File.WriteAllText(Path.Combine(filePath, fileName), fileContents);
		}

		/// <summary>
		/// Verifies if the directory exists at the path provided
		/// </summary>
		/// <param name="directoryPath"> The directory path being checked </param>
		private static void VerifyDirectoryExists(string directoryPath)
        {
			if (!Directory.Exists(directoryPath))
            {
				throw new DirectoryNotFoundException($"directory was not found for path {directoryPath}");
            }
        }
	}
}
