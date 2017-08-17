using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using GitServer.SyntaxHighlighing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace GitServer.Extensions
{
	public static class HtmlExtensions
    {
		public static IHtmlContent HighlightCode(this IHtmlHelper helper, string text, Highlighter highlighter, string cssClass = null)
		{
			TagBuilder rootTag = new TagBuilder("pre");

			if(cssClass != null)
				rootTag.AddCssClass(cssClass);

			StringBuilder builder = new StringBuilder();
			Dictionary<string, string> lastAttrs = null;
			foreach(HighlighterElement element in highlighter.Highlight(text))
			{
				if(element.Attributes.DictionaryEqual(lastAttrs) || string.IsNullOrWhiteSpace(element.Value))
				{
					builder.Append(element.Value);
				}
				else
				{
					if (lastAttrs != null)
					{
						TagBuilder elementTag = new TagBuilder("span");
						elementTag.Attributes.AddDictionary(lastAttrs);
						elementTag.InnerHtml.Append(builder.ToString());
						rootTag.InnerHtml.AppendHtml(elementTag);

						builder.Clear();
					}

					builder.Append(element.Value);
					lastAttrs = element.Attributes;
				}
			}

			TagBuilder lastElement = new TagBuilder("span");
			lastElement.Attributes.AddDictionary(lastAttrs);
			lastElement.InnerHtml.Append(builder.ToString());
			rootTag.InnerHtml.AppendHtml(lastElement);

			return rootTag;
		}

		public static string UnencodedRouteLink(this IUrlHelper helper, string routeName, object routeValues)
		{
			return WebUtility.UrlDecode(helper.RouteUrl(routeName, routeValues));
		}
	}
}
