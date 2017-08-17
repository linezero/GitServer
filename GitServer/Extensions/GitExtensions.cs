using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GitServer.Extensions
{
	public static class GitExtensions
    {
		public static string MakePackfile(this IEnumerable<string> lines)
			=> string.Join("\n", lines.Select(d => $"{d.Length:x4}{d}").Append("0000"));

		public static IEnumerable<string> ReadPackfile(this Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				string line;
				while (ReadPackfileLine(reader, out line) > 0)
					yield return line;
			}
		}

		private static int ReadPackfileLine(StreamReader reader, out string line)
		{
			line = null;
			char[] lengthChars = new char[4];
			if (reader.Read(lengthChars, 0, 4) < 4)
				return -1;

			int lineLength;
			if (!int.TryParse(new string(lengthChars), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lineLength))
				return -1;

			char[] lineChars = new char[lineLength];
			if (reader.Read(lineChars, 0, lineLength) < lineLength)
				return -1;

			line = new string(lineChars);
			reader.ReadLine();
			return lineLength;
		}

		public static IEnumerable<T> TraverseTree<T>(this T root, Func<T, IEnumerable<T>> childSelector)
		{
			Stack<T> nodes = new Stack<T>(new[] { root });
			while(nodes.Count > 0)
			{
				T node = nodes.Pop();
				yield return node;

				foreach (T child in childSelector(node))
					nodes.Push(child);
			}
		}
	}
}
