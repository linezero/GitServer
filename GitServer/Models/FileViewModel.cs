using LibGit2Sharp;

namespace GitServer.Models
{
	public class FileViewModel
	{
		private Repository _repository;
		private GitObject _object;
		private string _path;
		private string _name;

		public Repository Repository => _repository;
        public GitObject Object => _object;

		public string SHA1 => _object.Sha;
		public ObjectType Type => Repository.ObjectDatabase.RetrieveObjectMetadata(_object.Id).Type;
		public string Path => _path;
		public string Name => _name;

		protected internal FileViewModel(Repository repo, string path, string name, GitObject obj)
		{
			_repository = repo;
			_path = path;
			_name = name;
			_object = obj;
		}

		public static FileViewModel FromGitObject(Repository repo, string path, string name, GitObject obj)
		{
			switch(repo.ObjectDatabase.RetrieveObjectMetadata(obj.Id).Type)
			{
				case ObjectType.Blob:
					return new BlobModel(repo, path, name, (Blob)obj);

				case ObjectType.Tree:
					return new TreeModel(repo, path, name, (Tree)obj);

				default:
					return null;
			}
		}
	}

	public class FileViewModel<TObject> : FileViewModel where TObject : GitObject
    {
		protected new TObject Object => (TObject)base.Object;

		protected internal FileViewModel(Repository repo, string path, string name, TObject obj) : base(repo, path, name, obj) { }
	}
}
