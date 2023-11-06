using System.Threading;

namespace ECS
{
	public static class TypeId<T>
	{
		private static int _index;

		public static int Next()
		{
			Interlocked.Increment(ref _index);
			return _index - 1;
		}

		public static void Reset()
		{
			_index = 0;
		}
	}

	public static class TypeId<T, U>
	{
		private static bool _init;
		private static int _id;
		public static int Id
		{
			get
			{
				if (_init) return _id;
				_init = true;
				_id = TypeId<T>.Next();
				return _id;
			}
		}

		public static int NextId
		{
			get
			{
				if (_init) return _id;
				_init = true;
				_id = TypeId<T>.Next() + 1;
				return _id;
			}
		}
	}
}