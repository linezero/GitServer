using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GitServer.Extensions
{
	public static class HelperExtensions
    {
		public static string ToEnvVariable(this IEnumerable<string> stringList, string seperator = ",")
		{
			return string.Join(seperator, stringList.Select(d => d.Replace(@"\", @"\\").Replace(seperator, $@"\{seperator}")));
		}

		public static void CopyTo(this StreamReader reader, StreamWriter writer, int bufferSize = 4096)
		{
			char[] buffer = new char[bufferSize];
			int read;
			while((read = reader.Read(buffer, 0, bufferSize)) > 0)
			{
				writer.Write(buffer, 0, read);
			}
		}

		public static void AddDictionary<TValue>(this AttributeDictionary attrDictionary, IDictionary<string, TValue> dictionary)
		{
			foreach (KeyValuePair<string, TValue> kvp in dictionary)
				attrDictionary.Add(kvp.Key, kvp.Value.ToString());
		}

		public static bool DictionaryEqual<TKey, TValue>(
			this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
		{
			return first.DictionaryEqual(second, null);
		}

		public static bool DictionaryEqual<TKey, TValue>(
			this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second,
			IEqualityComparer<TValue> valueComparer)
		{
			if (first == second) return true;
			if ((first == null) || (second == null)) return false;
			if (first.Count != second.Count) return false;

			valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

			foreach (KeyValuePair<TKey, TValue> kvp in first)
			{
				TValue secondValue;
				if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
				if (!valueComparer.Equals(kvp.Value, secondValue)) return false;
			}
			return true;
		}
	}
}
