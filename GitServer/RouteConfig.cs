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
				"git/{repoName}.git",
				new { controller = "FileView", action = "RedirectGitLink" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetRepositoryHomeView",
				"git/{repoName}",
				new { controller = "FileView", action = "GetTreeView", id = "master", path = string.Empty },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetTreeView",
				"git/{repoName}/tree/{id}/{*path}",
				new { controller = "FileView", action = "GetTreeView" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetBlobView",
				"git/{repoName}/blob/{id}/{*path}",
				new { controller = "FileView", action = "GetBlobView" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);

			routeBuilder.MapRoute(
				"GetRawBlob",
				"git/{repoName}/raw/{id}/{*path}",
				new { controller = "FileView", action = "GetRawBlob" },
				new { method = new HttpMethodRouteConstraint("GET") }
			);
			#endregion
		}
    }
}
