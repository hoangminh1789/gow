using System;

namespace ECS
{
	public class Group : SparseSet, IEntityCollection
	{
		public Signal OnAdded { get; private set; }
		public Signal OnReplaced { get; private set; }
		public Signal OnPreRemoved { get; private set; }
		public Signal OnPostRemoved { get; private set; }

		private World _world;
		private Filter _filter;

		internal Group(World world, Filter filter, int[] entities, int entitySize)
		{
			_world = world;
			_filter = filter;
			OnAdded = new Signal();
			OnReplaced = new Signal();
			OnPreRemoved = new Signal();
			OnPostRemoved = new Signal();

			if (_filter.All.Length > 0)
			{
				Array.ForEach(_filter.All, (id) =>
				{
					var pool = _world.UnsafePool(id);
					pool.OnAdded.Bind(CheckAdd);
					pool.OnReplaced.Bind(CheckReplace);
					pool.OnPostRemoved.Bind(TryRemove);
				});
			}

			if (_filter.Any.Length > 0)
			{
				Array.ForEach(_filter.Any, (id) =>
				{
					var pool = _world.UnsafePool(id);
					pool.OnAdded.Bind(CheckAdd);
					pool.OnReplaced.Bind(CheckReplace);
					pool.OnPostRemoved.Bind(CheckRemove);
				});
			}

			if (_filter.None.Length > 0)
			{
				Array.ForEach(_filter.None, (id) =>
				{
					var pool = _world.UnsafePool(id);
					pool.OnAdded.Bind(TryRemove);
					pool.OnPostRemoved.Bind(CheckAdd);
				});
			}

			Add(entities, entitySize);
		}

		private void CheckAdd(int entity)
		{
			if (!Has(entity) && IsQualified(entity))
				Add(entity);
		}

		private void TryRemove(int entity)
		{
			if (Has(entity))
				Remove(entity);
		}

		private void CheckRemove(int entity)
		{
			if (Has(entity) && !IsQualified(entity))
				Remove(entity);
		}

		private void CheckReplace(int entity)
		{
			if (Has(entity))
				Replace(entity);
		}

		private new void Add(int entity)
		{
			base.Add(entity);
			OnAdded.Emit(entity);
		}

		private new void Remove(int entity)
		{
			OnPreRemoved.Emit(entity);
			base.Remove(entity);
			OnPostRemoved.Emit(entity);
		}

		private void Replace(int entity)
		{
			OnReplaced.Emit(entity);
		}

		private bool IsQualified(int entity)
		{
			if (_filter.All.Length > 0 && !_world.HasAll(entity, _filter.All))
				return false;

			if (_filter.None.Length > 0 && _world.HasAny(entity, _filter.None))
				return false;

			if (_filter.Any.Length > 0)
				if (!_world.HasAny(entity, _filter.Any))
				return false;

			return true;
		}
	}
}