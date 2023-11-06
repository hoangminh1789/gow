using System;
using System.Collections;
using System.Collections.Generic;

namespace ECS
{
	public class SparseSet : IEnumerable<int>
	{
		protected const int kDefaultCapacity = 4;
		protected int _size;
		protected int[][] _sparse;
		protected int[] _dense;

		public int Count => _size;

		public SparseSet()
		{
			_size = 0;
			_sparse = new int[0][];
			_dense = new int[0];

			EnsureSparse(0);
			EnsureDense(kDefaultCapacity);
		}

		public int this[int index] => _dense[index];

		public (int[], int) Raw()
		{
			return (_dense, _size);
		}

		public bool Has(int entity)
		{
			int page = Page(entity);
			int offset = Offset(entity);
			return page < _sparse.Length && _sparse[page] != null && _sparse[page][offset] != Constants.kNull;
		}

		public void Add(int entity)
		{
			int page = Page(entity);
			int offset = Offset(entity);

			EnsureSparse(page);
			EnsureDense(_size + 1);

			_sparse[page][offset] = _size;
			_dense[_size] = entity;

			_size += 1;
		}

		public void Add(int[] entities, int entitySize)
		{
			for (int i = 0; i < entitySize; i++)
			{
				int entity = entities[i];
				Add(entity);
			}
		}

		public void Remove(int entity)
		{
			int page = Page(entity);
			int offset = Offset(entity);
			int lastEntity = _dense[_size - 1];

			_sparse[Page(lastEntity)][Offset(lastEntity)] = _sparse[page][offset];
			_dense[_sparse[page][offset]] = lastEntity;
			_sparse[page][offset] = Constants.kNull;
			_size -= 1;
		}

		public void Clear()
		{
			_sparse = new int[0][];
			_size = 0;
		}

		public void EnsureSparse(int page)
		{
			int requireCapacity = page + 1;
			int oldCapacity = _sparse.Length;
			if (oldCapacity < requireCapacity)
			{
				Array.Resize(ref _sparse, requireCapacity);
			}

			if (_sparse[page] == null)
			{
				_sparse[page] = new int[Constants.kPageSize];
				for (int i = 0; i < Constants.kPageSize; i++)
				{
					_sparse[page][i] = Constants.kNull;
				}
			}
		}

		public void EnsureDense(int minCapacity)
		{
			int oldCapacity = _dense.Length;
			if (oldCapacity < minCapacity)
			{
				int newCapacity = oldCapacity > 0 ? oldCapacity * 2 : 1;
				Array.Resize(ref _dense, newCapacity);
			}
		}

		protected int Page(int entity)
		{
			return (entity >> Constants.kPageShift) & Constants.kPageMask;
		}

		protected int Offset(int entity)
		{
			return entity & Constants.kPageSize - 1;
		}

		public IEnumerator<int> GetEnumerator()
		{
			for (int i = 0; i < _size; i++)
			{
				yield return _dense[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}