using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECS
{
	public partial class World
	{
		private List<IInitialize> _initializeSystems = new List<IInitialize>();
		private List<IUpdate> _updateSystems = new List<IUpdate>();
		private List<IFixedUpdate> _fixedUpdateSystems = new List<IFixedUpdate>();
		private List<ILateUpdate> _lateUpdateSystems = new List<ILateUpdate>();
		private List<ICleanup> _cleanupSystems = new List<ICleanup>();
		private List<ComponentSystem> _systems = new List<ComponentSystem>();
		private Dictionary<ComponentSystem, FieldInfo[]> _systemFieldInfos = new Dictionary<ComponentSystem, FieldInfo[]>();
		

		public void AddSystem(params ComponentSystem[] systems)
		{
			foreach (var system in systems)
			{
				if (system is IInitialize initializeSystem)
				{
					_initializeSystems.Add(initializeSystem);
				}

				if (system is IUpdate updateSystem)
				{
					_updateSystems.Add(updateSystem);
				}

				if (system is IFixedUpdate fixedUpdateSystem)
				{
					_fixedUpdateSystems.Add(fixedUpdateSystem);
				}

				if (system is ILateUpdate lateUpdateSystem)
				{
					_lateUpdateSystems.Add(lateUpdateSystem);
				}

				if (system is ICleanup cleanupSystem)
				{
					_cleanupSystems.Add(cleanupSystem);
				}

				system.Initialize(this);
				_systems.AddRange(systems);
			}
		}

		public ComponentSystem GetSystem<T>() where T : ComponentSystem
		{
			return _systems.Find(system => system.GetType() == typeof(T));
		}

		public void Initialize()
		{
			_initializeSystems.ForEach(system => system.Initialize());
		}

		public void CleanUp()
		{
			_cleanupSystems.ForEach(system =>
			{
				system.Cleanup();
			});
		}

		public void Update()
		{
			_updateSystems.ForEach(system =>
			{
				system.Update();
			});
		}

		public void FixedUpdate()
		{
			_fixedUpdateSystems.ForEach(system =>
			{
				system.FixedUpdate();
			});
		}

		public void LateUpdate()
		{
			_lateUpdateSystems.ForEach(system =>
			{
				system.LateUpdate();
			});
		}

		/// <summary>
		/// Simple Injection. Can be optimize a little bit
		/// </summary>
		/// <param name="value">Inject value</param>
		/// <typeparam name="T">Inject type</typeparam>
		public void Inject<T>(object value)
		{
			Inject((T)value);
		}

		public void Inject<T>(T value)
		{
			var type = typeof(T);
			foreach (var system in _systems)
			{
				var fieldInfos = GetSystemFieldInfos(system);
				foreach (var fieldInfo in fieldInfos)
				{
					if (fieldInfo.FieldType == type)
					{
						fieldInfo.SetValue(system, value);
					}
				}
			}
		}

		private FieldInfo[] GetSystemFieldInfos(ComponentSystem system)
		{
			_systemFieldInfos.TryGetValue(system, out FieldInfo[] fieldInfos);

			if (fieldInfos == null)
			{
				var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
				fieldInfos = system.GetType().GetFields(bindingFlags);
				_systemFieldInfos[system] = fieldInfos;
			}
			
			return fieldInfos;
		}
	}
}