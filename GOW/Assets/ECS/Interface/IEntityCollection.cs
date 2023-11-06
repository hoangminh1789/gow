using System.Collections.Generic;

namespace ECS
{
	public interface IEntityCollection : IEnumerable<int>
	{
		Signal OnAdded { get; }
		Signal OnReplaced { get; }
		Signal OnPreRemoved { get; }
		Signal OnPostRemoved { get; }

		int Count { get; }
		int this[int index] { get; }
		bool Has(int id);
		(int[], int) Raw();
	}
}