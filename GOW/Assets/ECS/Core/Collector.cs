using System;
using System.Collections.Generic;

namespace ECS
{
	[Flags]
	public enum TriggerEvent
	{
		Added = 1,
		Replaced = 2,
		Removed = 4
	}

	public interface ICollector : IEntityCollection
	{
		void Clear();
		void Deactivate();
		ICollector Where(Func<int, bool> filter);
		ICollector Trigger(IEntityCollection triggerGroup, TriggerEvent triggerEvent);
	}

	public class Collector : SparseSet, ICollector
	{
		private class TriggerCollection
		{
			public IEntityCollection collection;
			public TriggerEvent evt;
		}

		public Signal OnAdded { get; private set; }

		public Signal OnReplaced { get; private set; }

		public Signal OnPreRemoved { get; private set; }
		public Signal OnPostRemoved { get; private set; }

		private World _world;
		private Func<int, bool> _filter;
		private List<TriggerCollection> _triggerCollections;

		public Collector(World world)
		{
			_world = world;
			_triggerCollections = new List<TriggerCollection>();

			OnAdded = new Signal();
			OnReplaced = new Signal();
			OnPreRemoved = new Signal();
			OnPostRemoved = new Signal();
		}

		public ICollector Where(Func<int, bool> filter)
		{
			_filter = filter;
			return this;
		}

		public ICollector Trigger(IEntityCollection collection, TriggerEvent evt)
		{
			if (evt.HasFlag(TriggerEvent.Added))
			{
				collection.OnAdded.Bind(CheckAdded);
			}

			if (evt.HasFlag(TriggerEvent.Replaced))
			{
				collection.OnReplaced.Bind(CheckAdded);
			}

			if (evt.HasFlag(TriggerEvent.Removed))
			{
				collection.OnPreRemoved.Bind(CheckAdded);
			}

			_triggerCollections.Add(new TriggerCollection { collection = collection, evt = evt });
			return this;
		}

		private void CheckAdded(int entity)
		{
			if (_filter != null && !_filter.Invoke(entity))
				return;
			
			if (!Has(entity))
				Add(entity);
		}

		public void Deactivate()
		{
			foreach (var item in _triggerCollections)
			{
				if (item.evt.HasFlag(TriggerEvent.Added))
				{
					item.collection.OnAdded.Unbind(CheckAdded);
				}

				if (item.evt.HasFlag(TriggerEvent.Replaced))
				{
					item.collection.OnReplaced.Unbind(CheckAdded);
				}

				if (item.evt.HasFlag(TriggerEvent.Removed))
				{
					item.collection.OnPreRemoved.Unbind(CheckAdded);
				}
			}
		}

		public new void Add(int entity)
		{
			base.Add(entity);
			OnAdded.Emit(entity);
		}
	}
}