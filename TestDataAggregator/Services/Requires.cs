using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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
		public static void NotNull(object value, string message = "")
		{
			bool isNull;

			if (value?.GetType() == typeof(string))
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

		/// <summary>
		/// Checks if the value is in the value set provided
		/// </summary>
		/// <param name="value"> The value to check if in the set</param>
		/// <param name="valueSet"> The value set to compare to</param>
		/// <param name="message"> The message to throw if the value is not in the set </param>
		public static void ValuesIn(object value, IEnumerable<object> valueSet, string message = "")
        {
			if (!valueSet.Any(x => x.Equals(value)))
            {
				throw new ArgumentException(message);
			}
        }
	}
}
