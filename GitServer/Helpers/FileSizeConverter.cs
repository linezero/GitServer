using System;
using System.Globalization;

namespace GitServer.Helpers
{
	public static class FileSizeConverter
    {
		public static string ToReadableFormat(long size, IFormatProvider formatProvider)
		{
			FormattableString result = $"{size:0,0.0} Bytes";

			if(size > (1024 * 1024))
			{
				result = $"{size / (1024d * 1024d):0,0.0} MB";
			}
			else if (size > 1024)
			{
				result = $"{size / 1024d:0,0.0} KB";
			}

			return result.ToString(formatProvider);
		}

		public static string ToReadableFormat(long size) => ToReadableFormat(size, CultureInfo.InvariantCulture);
    }
}
