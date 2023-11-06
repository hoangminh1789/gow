using System;

namespace ECS
{
	public interface IComponentPool : IEntityCollection
	{
		void Remove(int id);
		void Clear();
	}

	public class ComponentPool<T> : SparseSet, IComponentPool
	{
		private T[] _items;
		private int _id;
		public int Id => _id;

		public Signal OnAdded { get; private set; }
		public Signal OnReplaced { get; private set; }
		public Signal OnPreRemoved { get; private set; }
		public Signal OnPostRemoved { get; private set; }

		public ComponentPool(int id = default) : base()
		{
			_id = id;
			_items = new T[0];
			OnAdded = new Signal();
			OnReplaced = new Signal();
			OnPreRemoved = new Signal();
			OnPostRemoved = new Signal();
		}

		public T Get(int entity)
		{
			return _items[_sparse[Page(entity)][Offset(entity)]];
		}

		public ref T GetRef(int entity)
		{
			return ref _items[_sparse[Page(entity)][Offset(entity)]];
		}

		public void Add(int entity, T item = default)
		{
			base.Add(entity);
			EnsureItem();
			_items[_size - 1] = item;

			OnAdded.Emit(entity);
		}

		public void Replace(int entity, T item)
		{
			_items[_sparse[Page(entity)][Offset(entity)]] = item;
			OnReplaced.Emit(entity);
		}

		public void MarkChanged(int[] entities, int size)
		{
			if (!OnReplaced.HasSub)
				return;

			for (int i = 0; i < size; i++)
			{
				OnReplaced.Emit(entities[i]);
			}
		}

		public new void Remove(int entity)
		{
			if (!Has(entity))
				return;
			
			OnPreRemoved.Emit(entity);
			_items[_sparse[Page(entity)][Offset(entity)]] = _items[_size - 1];
			base.Remove(entity);
			OnPostRemoved.Emit(entity);
		}

		public new void Clear()
		{
			_items = new T[0];
			base.Clear();
		}

		public void RemoveAll()
		{
			if (OnPreRemoved.HasSub)
			{
				for (int i = 0; i < _size; i++)
				{
					OnPreRemoved.Emit(_dense[i]);
				}
			}
			
			Clear();

			if (OnPostRemoved.HasSub)
			{
				for (int i = 0; i < _size; i++)
				{
					OnPostRemoved.Emit(_dense[i]);
				}
			}
		}

		public (T[], int) ItemRaw()
		{
			return (_items, _size);
		}

		public void EnsureItem()
		{
			Array.Resize(ref _items, _dense.Length);
		}
	}
}