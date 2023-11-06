using System;

namespace ECS
{
	public class Filter
	{
		private static int[] _emptyArray = new int[0];
		internal int[] All { get; private set; }
		internal int[] Any { get; private set; }
		internal int[] None { get; private set; }

		public Filter(int[] all, int[] any, int[] none)
		{
			All = all ?? _emptyArray;
			Any = any ?? _emptyArray;
			None = none ?? _emptyArray;
		}
	}
}