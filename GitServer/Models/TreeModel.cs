using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace GitServer.Models
{
	public class TreeModel : FileViewModel<Tree>
	{
		private string _parent;

		public string Parent => _parent;
		public IEnumerable<FileViewModel> Children => Object.Select(d => FromGitObject(Repository, d.Path, d.Name, d.Target));

		public TreeModel(Repository repo, string path, string name, Tree obj, string parent = null) : base(repo, path, name, obj)
		{
			_parent = parent;
		}
	}
}
