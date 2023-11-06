namespace ECS
{
	public partial class World
	{
		public World()
		{
			var settings = new WorldSettings
			{
				defaultEntityInstance = 1
			};

			InitEntity(settings);
			InitComponent(settings);
			InitGroup(settings);
		}
	}
}