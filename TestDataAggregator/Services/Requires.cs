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
		/// <param name="messageException"> The message to throw if the value is null </param>
		/// <param name="propertyName"> The name of the property </param>
		public static void NotNull(object value, string propertyName, Action<string> messageException)
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
				messageException(propertyName);
			}
		}
	}
}
