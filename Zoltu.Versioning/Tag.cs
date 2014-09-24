using System;
using System.Diagnostics.Contracts;

namespace Zoltu.Versioning
{
	public class Tag
	{
		public String Name
		{
			get
			{
				Contract.Ensures(Contract.Result<String>() != null);
				return _name;
			}
		}
		private readonly String _name;

		public String Sha
		{
			get
			{
				Contract.Ensures(Contract.Result<String>() != null);
				return _sha;
			}
		}
		private readonly String _sha;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_name != null);
			Contract.Invariant(_sha != null);
		}

		public static Tag TryCreate(LibGit2Sharp.Tag gitTag)
		{
			if (gitTag == null)
				return null;

			var target = gitTag.Target;
			if (target == null)
				return null;

			var sha = target.Sha;
			if (sha == null)
				return null;

			var name = gitTag.Name;
			if (name == null)
				return null;

			return new Tag(name, sha);
		}

		public Tag(String name, String sha)
		{
			Contract.Requires(name != null);
			Contract.Requires(sha != null);

			_name = name;
			_sha = sha;
		}
	}
}
