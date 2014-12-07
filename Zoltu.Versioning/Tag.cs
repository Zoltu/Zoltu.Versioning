using System;
using System.Diagnostics.Contracts;

namespace Zoltu.Versioning
{
	public class Tag
	{
		public String Name { get; private set; }
		public String Sha { get; private set; }

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(Name != null);
			Contract.Invariant(Sha != null);
		}

		public Tag(String name, String sha)
		{
			Contract.Requires(name != null);
			Contract.Requires(sha != null);

			Name = name;
			Sha = sha;
		}
	}
}
