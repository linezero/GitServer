using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GitServer.SyntaxHighlighing
{
	public class HighlighterElement
    {
		public string Value { get; set; }
		public Dictionary<string, string> Attributes { get; set; }

		public HighlighterElement(string value, IDictionary<string, string> attributes)
		{
			Value = value;
			Attributes = new Dictionary<string, string>(attributes);
		}
    }
}
