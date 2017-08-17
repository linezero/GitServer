using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GitServer.SyntaxHighlighing
{
	public class Highlighter
	{
		public List<HighlighterRule> Rules { get; set; }

		public Highlighter()
		{
			Rules = new List<HighlighterRule>();
			Rules.Add(new HighlighterRule(@"^."));
		}

		public Highlighter(IEnumerable<HighlighterRule> rules)
		{
			Rules = new List<HighlighterRule>(rules);
			Rules.Add(new HighlighterRule(@"^."));
		}

		public Highlighter(params HighlighterRule[] rules) : this((IEnumerable<HighlighterRule>)rules) { }

		public IEnumerable<HighlighterElement> Highlight(string text)
		{
			int i = 0;
			string str = text;
			int length = str.Length;

			Match match;
			while (i < length)
			{
				foreach (HighlighterRule rule in Rules)
				{
					match = rule.Pattern.Match(str);

					if (match.Success)
					{
						if (match.Length == 0)
							throw new Exception($"Rule {rule.Pattern} produced zero length result. Please modify it to not allow zero length results.");

						yield return new HighlighterElement(match.Value, rule.Attributes);
						i += match.Length;

						str = str.Remove(0, match.Length);
						break;
					}
				}
			}
		}

		public static Highlighter FromExtension(string extension)
		{
			switch(extension)
			{
				case ".cs":
				case ".java":
					return CSharpHighlighter;

				default:
					return NoHighlighter;
			}
		}

		public static Highlighter NoHighlighter => _noHighlighter;
		private static Highlighter _noHighlighter = new Highlighter();

		public static Highlighter CSharpHighlighter => _cSharpHighlighter;
		private static Highlighter _cSharpHighlighter = new Highlighter(
			new HighlighterRule(@"^\/\/.*?\n") { { "class", "comment" } },						//Line comments
			new HighlighterRule(@"^\/\*.*?\*\/") { { "class", "comment" } },					//Muliline comments
			new HighlighterRule(@"^""[^""\\]*(\\.[^""\\]*)*""") { { "class", "string" } },		//String literals
			new HighlighterRule(@"^\s"),														//Whitespace
			new HighlighterRule(@"^\b(bool|byte|sbyte|char|decimal|double|float|int|uint|long|ulong|new|object|short|ushort|string|base|this|void)\b") { { "class", "keyword" } },	//Keywords
			new HighlighterRule(@"^\b(as|break|case|catch|checked|continue|default|do|else|finally|fixed|for|foreach|goto|if|is|lock|return|switch|throw|try|unchecked|while|yield)\b") { { "class", "keyword" } },	//More keywords
			new HighlighterRule(@"^\b(abstract|class|const|delegate|enum|event|explicit|extern|get|implicit|in|internal|interface|nameof|namespace|operator|out|override|params|partial|private|protected|public|readonly|ref|sealed|set|sizeof|static|struct|typeof|using|virtual|volatile)\b") { { "class", "keyword" } }	//Even more keywords
		);
	}
}
