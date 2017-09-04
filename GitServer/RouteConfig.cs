using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace GitServer
{
	public static class RouteConfig
    {
		public static void RegisterRoutes(IRouteBuilder routeBuilder)
		{
			routeBuilder.MapRoute(
				"Home",
                "{controller=Home}/{action=Home}/{id?}"
			);

			#region Routes for viewing the file tree
			routeBuilder.MapRoute(
				"RedirectGitLink",
				"{userName}/{repoName}.git",
				new { controller = "FileView", action = "RedirectGitLink" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetRepositoryHomeView",
                "{userName}/{repoName}",
				new { controller = "FileView", action = "GetTreeView", id = "master", path = string.Empty },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetTreeView",
                "{userName}/{repoName}/tree/{id}/{*path}",
				new { controller = "FileView", action = "GetTreeView" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetBlobView",
                "{userName}/{repoName}/blob/{id}/{*path}",
				new { controller = "FileView", action = "GetBlobView" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetRawBlob",
                "{userName}/{repoName}/raw/{id}/{*path}",
				new { controller = "FileView", action = "GetRawBlob" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);
			#endregion
		}
    }
}
