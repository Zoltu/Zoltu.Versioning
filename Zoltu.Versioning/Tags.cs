using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Zoltu.Versioning
{
	public class Tags
	{
		private readonly IDictionary<String, Tag> _tags;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_tags != null);
		}

		public Tags(LibGit2Sharp.TagCollection gitTags)
		{
			_tags = GetTagsFromTagCollection(gitTags);
		}

		public Tags(LibGit2Sharp.IRepository gitRepository)
		{
			if (gitRepository == null)
				_tags = new Dictionary<String, Tag>();
			else
				_tags = GetTagsFromTagCollection(gitRepository.Tags);
		}

		private IDictionary<String, Tag> GetTagsFromTagCollection(LibGit2Sharp.TagCollection gitTags)
		{
			if (gitTags == null)
				return new Dictionary<String, Tag>();
			else
				return gitTags
					.Select(gitTag => Tag.TryCreate(gitTag))
					.Where(tag => tag != null)
					.ToDictionary(tag => tag.Sha);
		}

		public Boolean ContainsKey(String key)
		{
			if (key == null)
				return false;

			return _tags.ContainsKey(key);
		}

		public Tag TryGet(String key)
		{
			if (key == null)
				return null;

			Tag value;
			_tags.TryGetValue(key, out value);
			return value;
		}

		public Tag TryGet(LibGit2Sharp.Commit commit)
		{
			if (commit == null)
				return null;

			return TryGet(commit.Sha);
		}
	}
}
