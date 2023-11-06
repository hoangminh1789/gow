namespace ECS
{
	public interface IInitialize
	{
		void Initialize();
	}

	public interface ICleanup
	{
		void Cleanup();
	}

	public interface IUpdate
	{
		void Update();
	}

	public interface ILateUpdate
	{
		void LateUpdate();
	}

	public interface IFixedUpdate
	{
		void FixedUpdate();
	}

	public class ComponentSystem
	{
		protected World _world;

		internal void Initialize(World world)
		{
			_world = world;
		}
	}
}