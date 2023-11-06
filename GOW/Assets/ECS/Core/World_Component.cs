using System;

namespace ECS
{
	public partial class World
	{
		private IComponentPool[] _pools;

		private void InitComponent(WorldSettings settings)
		{
			_pools = new IComponentPool[0];
		}

		internal ComponentPool<T> Pool<T>()
		{
			int id = TypeId<IComponentPool, T>.Id;
			if (id >= _pools.Length)
			{
				int newCapacity = id + 1;
				ArrayUtils.Ensure(ref _pools, newCapacity);
				_pools[id] = new ComponentPool<T>(id);
			}

			return (ComponentPool<T>)_pools[id];
		}

		internal IComponentPool UnsafePool(int id)
		{
			return _pools[id];
		}

		public void Add<C1>(int entity, C1 c1 = default)
		{
			if (Pool<C1>().Has(entity))
			{
				UnityEngine.Debug.LogError($"Entity already has {c1.GetType().Name}");
			}
			Pool<C1>().Add(entity, c1);
		}

		public void Add<C1, C2>(int entity, C1 c1, C2 c2)
		{
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
		}

		public void Add<C1, C2, C3>(int entity, C1 c1, C2 c2, C3 c3)
		{
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
			Pool<C3>().Add(entity, c3);
		}

		public void Add<C1, C2, C3, C4>(int entity, C1 c1, C2 c2, C3 c3, C4 c4)
		{
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
			Pool<C3>().Add(entity, c3);
			Pool<C4>().Add(entity, c4);
		}

		public void Add<C1, C2, C3, C4, C5>(int entity, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5)
		{
			Pool<C1>().Add(entity, c1);
			Pool<C2>().Add(entity, c2);
			Pool<C3>().Add(entity, c3);
			Pool<C4>().Add(entity, c4);
			Pool<C5>().Add(entity, c5);
		}

		public void Replace<C1>(int entity, C1 c1)
		{
			Pool<C1>().Replace(entity, c1);
		}

		public void Replace<C1, C2>(int entity, C1 c1, C2 c2)
		{
			Pool<C1>().Replace(entity, c1);
			Pool<C2>().Replace(entity, c2);
		}

		public void Replace<C1, C2, C3>(int entity, C1 c1, C2 c2, C3 c3)
		{
			Pool<C1>().Replace(entity, c1);
			Pool<C2>().Replace(entity, c2);
			Pool<C3>().Replace(entity, c3);
		}

		public void AddOrReplace<C1>(int entity, C1 c1)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Add(entity, c1);
		}

		public void AddIfNotExist<C1>(int entity, C1 c1)
		{
			var pool1 = Pool<C1>();
			if (!pool1.Has(entity))
				pool1.Add(entity, c1);
		}

		public void AddOrReplace<C1, C2>(int entity, C1 c1, C2 c2)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Add(entity, c1);

			var pool2 = Pool<C2>();
			if (pool2.Has(entity))
				pool2.Add(entity, c2);
		}

		public void AddOrReplace<C1, C2, C3>(int entity, C1 c1, C2 c2, C3 c3)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Add(entity, c1);

			var pool2 = Pool<C2>();
			if (pool2.Has(entity))
				pool2.Add(entity, c2);

			var pool3 = Pool<C3>();
			if (pool3.Has(entity))
				pool3.Add(entity, c3);
		}

		public bool Has<C1>(int entity)
		{
			return Pool<C1>().Has(entity);
		}

		public bool Has<C1, C2>(int entity)
		{
			return Pool<C1>().Has(entity) && Pool<C2>().Has(entity);
		}

		public bool Has<C1, C2, C3>(int entity)
		{
			return Pool<C1>().Has(entity) && Pool<C2>().Has(entity) && Pool<C3>().Has(entity);
		}

		public bool HasAny<C1, C2>(int entity)
		{
			return Pool<C1>().Has(entity) || Pool<C2>().Has(entity);
		}

		public bool HasAny<C1, C2, C3>(int entity)
		{
			return Pool<C1>().Has(entity) || Pool<C2>().Has(entity) || Pool<C3>().Has(entity);
		}

		internal bool HasAll(int entity, params int[] componentIds)
		{
			foreach (var componentId in componentIds)
			{
				if (!_pools[componentId].Has(entity))
					return false;
			}
			
			return true;
		}

		internal bool HasAny(int entity, params int[] componentIds)
		{
			foreach (var componentId in componentIds)
			{
				if (_pools[componentId].Has(entity))
					return true;
			}

			return false;
		}

		public C1 Get<C1>(int entity)
		{
			return Pool<C1>().Get(entity);
		}

		public void Get<C1>(int entity, out C1 c1)
		{
			c1 = Pool<C1>().Get(entity);
		}

		public void Get<C1, C2>(int entity, out C1 c1, out C2 c2)
		{
			c1 = Pool<C1>().Get(entity);
			c2 = Pool<C2>().Get(entity);
		}

		public void Get<C1, C2, C3>(int entity, out C1 c1, out C2 c2, out C3 c3)
		{
			c1 = Pool<C1>().Get(entity);
			c2 = Pool<C2>().Get(entity);
			c3 = Pool<C3>().Get(entity);
		}

		public ref C1 GetRef<C1>(int entity)
		{
			return ref Pool<C1>().GetRef(entity);
		}

		public void Remove<C1>(int entity)
		{
			Pool<C1>().Remove(entity);
		}

		public void Remove<C1, C2>(int entity)
		{
			Pool<C1>().Remove(entity);
			Pool<C2>().Remove(entity);
		}

		public void Remove<C1, C2, C3>(int entity)
		{
			Pool<C1>().Remove(entity);
			Pool<C2>().Remove(entity);
			Pool<C3>().Remove(entity);
		}

		public void RemoveIfExist<C1>(int entity)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Remove(entity);
		}

		public void RemoveIfExist<C1, C2>(int entity)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Remove(entity);

			var pool2 = Pool<C2>();
			if (pool2.Has(entity))
				pool2.Remove(entity);
		}

		public void RemoveIfExist<C1, C2, C3>(int entity)
		{
			var pool1 = Pool<C1>();
			if (pool1.Has(entity))
				pool1.Remove(entity);

			var pool2 = Pool<C2>();
			if (pool2.Has(entity))
				pool2.Remove(entity);

			var pool3 = Pool<C3>();
			if (pool3.Has(entity))
				pool3.Remove(entity);
		}

		public void Clear<C1>()
		{
			Pool<C1>().RemoveAll();
		}
	}
}