using System;
using UnityEngine.Assertions;

namespace ECS
{
	public partial class World
	{
		private int[] _entities;
		private int _recycleEntity;
		private int _uniqueEntity;
		private int _totalEntity;

		public int TotalEntity => _totalEntity - 1;
		public int UniqueEntity => _uniqueEntity;

		private void InitEntity(WorldSettings settings)
		{
			_entities = new int[0];
			_totalEntity = 0;
			_recycleEntity = Constants.kNull;

			EnsureEntityCapacity(settings.defaultEntityInstance);
			_uniqueEntity = CreateEntity();
		}

		public (int[], int) EntityRaw()
		{
			return (_entities, _totalEntity);
		}

		private void EnsureEntityCapacity(int minCapacity)
		{
			int oldCapacity = _entities.Length;
			if (oldCapacity < minCapacity)
			{
				int newCap = oldCapacity > 0 ? oldCapacity : 1;
				do
				{
					newCap *= 2;
				} while (newCap < minCapacity);
				Array.Resize(ref _entities, newCap);

				for (int i = oldCapacity; i < newCap; i++)
				{
					_entities[i] = Constants.kNull;
				}
			}
		}

		public bool IsValid(int entity)
		{
			int pos = entity.Position();
			if (pos >= 0 && pos < _totalEntity)
			{
				return entity.Version() == _entities[pos].Version();
			}

			return false;
		}

		public int CreateEntity()
		{
			if (_recycleEntity != Constants.kNull)
			{
				var entity = _recycleEntity;
				_recycleEntity = _entities[_recycleEntity];
				_entities[entity] = entity;

				_totalEntity += 1;
				return entity;
			}
			else
			{
				EnsureEntityCapacity(_totalEntity + 1);
				_entities[_totalEntity] = _totalEntity;
				return _entities[_totalEntity++];
			}
		}

		public int CreateEntity<C1>(C1 c1)
		{
			int entity = CreateEntity();
			Pool<C1>().Add(entity, c1);
			return entity;
		}

		public int CreateEntity<C1, C2>(C1 c1, C2 c2)
		{
			int entity = CreateEntity();
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
			return entity;
		}

		public int CreateEntity<C1, C2, C3>(C1 c1, C2 c2, C3 c3)
		{
			int entity = CreateEntity();
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
			Pool<C3>().Add(entity, c3);
			return entity;
		}

		public void DestroyEntity(int entity)
		{
			Assert.IsFalse(entity == _uniqueEntity, "Can not destroy unique entity!");
			
			// int pos = entity.Position();
			// int version = entity.Version() + 0; // increase version to 1 unit

			for (int i = 0, size = _pools.Length; i < size; i++)
			{
				if (_pools[i].Has(entity))
				{
					_pools[i].Remove(entity);
				}
			}
			// _recycleEntity = pos | (version << Constants.kVersionShift);
			_entities[entity] = _recycleEntity;
			_recycleEntity = entity;
			_totalEntity -= 1;
		}

		public void DestroyEntities(IEntityCollection collection)
		{
			var (entities, size) = collection.Raw();
			for (int i = size - 1; i >= 0; --i)
			{
				DestroyEntity(entities[i]);
			}
		}

		public void ClearAll()
		{
			_totalEntity = 0;
			_entities = new int[0];
			foreach (var pool in _pools)
			{
				pool.Clear();
			}

			foreach (var group in _cacheGroups.Values)
			{
				group.Clear();
			}
			_cacheGroups.Clear();
		}
	}
}