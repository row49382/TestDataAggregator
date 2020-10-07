using System;

namespace TestInformationAggregator.Services
{
	/// <summary>
	/// Service to hold logic for requirements of the application
	/// </summary>
	public static class Requires
	{
		/// <summary>
		/// Checks if the value is null
		/// </summary>
		/// <param name="value"> The value to check</param>
		/// <param name="message"> The message to throw if the value is null </param>
		public static void NotNull(object value, string message)
		{
			bool isNull;

			if (value.GetType() == typeof(string))
			{
				isNull = string.IsNullOrEmpty((string)value);
			}
			else
			{
				isNull = value == null;
			}

			if (isNull)
			{
				throw new ArgumentException(message);
			}
		}
	}
}
