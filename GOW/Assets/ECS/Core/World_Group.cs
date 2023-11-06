using System;
using System.Collections.Generic;

namespace ECS
{
	public partial class World
	{
		private SparseSet _checkingEntities;
		private Dictionary<int, Group> _cacheGroups;
		
		private void InitGroup(WorldSettings settings)
		{
			_checkingEntities = new SparseSet();
			_cacheGroups = new Dictionary<int, Group>();
		}

		private (int[] entities, int size) Collect(Filter filter)
		{
			_checkingEntities.Clear();

			if (filter.All.Length > 0)
			{
				int minPoolId = GetSmallestPool(filter.All);
				var (entities, foundSize) = _pools[minPoolId].Raw();

				_checkingEntities.Add(entities, foundSize);

				FilterHasAll(_checkingEntities, filter.All, minPoolId);
				FilterHasNone(_checkingEntities, filter.None);
				FilterHasAny(_checkingEntities, filter.Any);
			}
			else if (filter.Any.Length > 0)
			{
				CollectAllAny(_checkingEntities, filter.Any);
				FilterHasNone(_checkingEntities, filter.None);
			}

			return _checkingEntities.Raw();
		}

		private int GetSmallestPool(int[] componentIds)
		{
			int minId = -1;
			int minValue = int.MaxValue;

			for (int i = 0, size = componentIds.Length; i < size; i++)
			{
				int id = componentIds[i];
				int count = _pools[id].Count;
				if (count < minValue)
				{
					minValue = count;
					minId = id;
				}
			}

			return minId;
		}

		private void FilterHasAll(SparseSet checkingSet, int[] componentIds, int excludeId = -1)
		{
			foreach (var poolId in componentIds)
			{
				if (poolId == excludeId)
					continue;

				var pool = _pools[poolId];
				for (int i = checkingSet.Count - 1; i >= 0; i--)
				{
					if (!pool.Has(checkingSet[i]))
						checkingSet.Remove(checkingSet[i]);
				}
			}
		}

		private void FilterHasAny(SparseSet checkingSet, int[] componentIds, int excludeId = -1)
		{
			if (componentIds.Length == 0)
				return;
			
			for (int i = checkingSet.Count - 1; i >= 0; i--)
			{
				bool hasComponent = false;
				foreach (var componentId in componentIds)
				{
					if (componentId == excludeId)
						continue;

					var pool = _pools[componentId];
					if (!pool.Has(checkingSet[i]))
					{
						hasComponent = true;
						break;
					}
				}

				if (!hasComponent)
					checkingSet.Remove(checkingSet[i]);
			}
		}

		private void FilterHasNone(SparseSet checkingSet, int[] componentIds)
		{
			foreach (var componentId in componentIds)
			{
				var pool = _pools[componentId];
				for (int i = checkingSet.Count - 1; i >= 0; i--)
				{
					if (pool.Has(checkingSet[i]))
						checkingSet.Remove(checkingSet[i]);
				}
			}
		}

		private void CollectAllAny(SparseSet checkingSet, int[] componentIds)
		{
			foreach (var componentId in componentIds)
			{
				var pool = _pools[componentId];
				var (dense, size) = pool.Raw();

				for (int i = 0; i < size; i++)
				{
					if (!checkingSet.Has(dense[i]))
						checkingSet.Add(dense[i]);
				}
			}
		}

		private void ConvertToEntities(SparseSet checkingSet, ref int[] entities)
		{
			ArrayUtils.Ensure(ref entities, checkingSet.Count);
			for (int i = 0; i < checkingSet.Count; i++)
			{
				entities[i] = _entities[checkingSet[i]];
			}
		}

		public IEntityCollection All<C1>()
		{
			return Pool<C1>();
		}

		public IEntityCollection All<C1, C2>()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<All<C1, C2>>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			new All<C1, C2>();
			if (group == null)
			{
				var filter = new FilterBuilder<All<C1, C2>>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection All<C1, C2, C3>()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<All<C1, C2, C3>>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			new All<C1, C2, C3>();
			if (group == null)
			{
				var filter = new FilterBuilder<All<C1, C2, C3>>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection Any<C1, C2>()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<EmptyFilter, Any<C1, C2>>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			new Any<C1, C2>();
			if (group == null)
			{
				var filter = new FilterBuilder<EmptyFilter, Any<C1, C2>>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection Any<C1, C2, C3>()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<EmptyFilter, Any<C1, C2, C3>>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			new Any<C1, C2, C3>();
			
			if (group == null)
			{
				var filter = new FilterBuilder<EmptyFilter, Any<C1, C2, C3>>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection Group<TFilterBuilder>() where TFilterBuilder : IFilterBuilder, new()
		{
			int filterId = TypeId<IFilterBuilder, TFilterBuilder>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			if (group == null)
			{
				var filter = new TFilterBuilder().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection Group<TAll, TAny>() where TAll : IAll, new() where TAny : IAny, new()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<TAll, TAny>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);
			if (group == null)
			{
				var filter = new FilterBuilder<TAll, TAny>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public IEntityCollection Group<TAll, TAny, TNone>() where TAll : IAll, new()
																	where TAny : IAny, new()
																	where TNone : INone, new()
		{
			int filterId = TypeId<IFilterBuilder, FilterBuilder<TAll, TAny, TNone>>.NextId;
			_cacheGroups.TryGetValue(filterId, out Group group);

			if (group == null)
			{
				var filter = new FilterBuilder<TAll, TAny, TNone>().Build(this);
				var (entities, size) = Collect(filter);
				_cacheGroups[filterId] = group = new Group(this, filter, entities, size);
			}

			return group;
		}

		public ICollector CreateCollector()
		{
			return new Collector(this);
		}
	}
}