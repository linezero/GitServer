using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GitServer.SyntaxHighlighing
{
	public class HighlighterRule : IDictionary<string, string>
    {
		public Regex Pattern { get; set; }
		public Dictionary<string, string> Attributes { get; set; }

		public ICollection<string> Keys => ((IDictionary<string, string>)Attributes).Keys;

		public ICollection<string> Values => ((IDictionary<string, string>)Attributes).Values;

		public int Count => ((IDictionary<string, string>)Attributes).Count;

		public bool IsReadOnly => ((IDictionary<string, string>)Attributes).IsReadOnly;

		public string this[string key] { get => ((IDictionary<string, string>)Attributes)[key]; set => ((IDictionary<string, string>)Attributes)[key] = value; }

		public HighlighterRule(Regex pattern)
		{
			Pattern = pattern;
			Attributes = new Dictionary<string, string>();
		}

		public HighlighterRule(string patternString) : this(new Regex(patternString, RegexOptions.Compiled | RegexOptions.Singleline)) { }

		public HighlighterRule(Regex pattern, IDictionary<string, string> attributes)
		{
			Pattern = pattern;
			Attributes = new Dictionary<string, string>(attributes);
		}

		public HighlighterRule(string patternString, IDictionary<string, string> attributes)
			: this(new Regex(patternString, RegexOptions.Compiled | RegexOptions.Singleline), attributes) { }

		public void Add(string key, string value)
		{
			((IDictionary<string, string>)Attributes).Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, string>)Attributes).ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, string>)Attributes).Remove(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return ((IDictionary<string, string>)Attributes).TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, string> item)
		{
			((IDictionary<string, string>)Attributes).Add(item);
		}

		public void Clear()
		{
			((IDictionary<string, string>)Attributes).Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return ((IDictionary<string, string>)Attributes).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((IDictionary<string, string>)Attributes).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			return ((IDictionary<string, string>)Attributes).Remove(item);
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ((IDictionary<string, string>)Attributes).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, string>)Attributes).GetEnumerator();
		}
	}
}
