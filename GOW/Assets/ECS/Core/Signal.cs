using System;

namespace ECS
{
	public delegate void Subscriber(int entity);

	public class Signal
	{
		private Subscriber[] _subs;
		private int _size;

		public bool HasSub => _size > 0;

		public Signal()
		{
			_subs = new Subscriber[0];
		}

		public void Bind(Subscriber sub)
		{
			ArrayUtils.Ensure(ref _subs, _size + 1);
			_subs[_size++] = sub;
		}

		public void Unbind(Subscriber sub)
		{
			int index = Array.IndexOf(_subs, sub);
			if (index < 0)
				return;
			
			int lastIndex = _size - 1;
			_subs[index] = _subs[lastIndex];
			_size -= 1;
		}

		public void Emit(int entity)
		{
			if (_subs.Length == 0)
				return;
			
			for (int i = 0; i < _size; i++)
			{
				_subs[i].Invoke(entity);
			}
		}
	}
}