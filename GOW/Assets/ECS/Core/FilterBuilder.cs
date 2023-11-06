namespace ECS
{
	public interface IFilter
	{
		int[] GetIds(World world);
	}

	public interface IAll : IFilter { }
	public interface IAny : IFilter { }
	public interface INone : IFilter { }

	public class Filter<C1>
	{
		public int[] GetIds(World world)
		{
			return new int[]
			{
				world.Pool<C1>().Id 
			};
		}
	}

	public class Filter<C1, C2>
	{
		public int[] GetIds(World world)
		{
			return new int[] 
			{
				world.Pool<C1>().Id,
				world.Pool<C2>().Id
			};
		}
	}

	public class Filter<C1, C2, C3>
	{
		public int[] GetIds(World world)
		{
			return new int[]
			{
				world.Pool<C1>().Id,
				world.Pool<C2>().Id,
				world.Pool<C3>().Id
			};
		}
	}

	public class Filter<C1, C2, C3, C4>
	{
		public int[] GetIds(World world)
		{
			return new int[]
			{
				world.Pool<C1>().Id,
				world.Pool<C2>().Id,
				world.Pool<C3>().Id,
				world.Pool<C4>().Id
			};
		}
	}

	public class Filter<C1, C2, C3, C4, C5>
	{
		public int[] GetIds(World world)
		{
			return new int[]
			{
				world.Pool<C1>().Id,
				world.Pool<C2>().Id,
				world.Pool<C3>().Id,
				world.Pool<C4>().Id,
				world.Pool<C5>().Id
			};
		}
	}

	public class EmptyFilter : IAll, IAny
	{
		private static int[] kEmptyArr = new int[0];
		public int[] GetIds(World world)
		{
			return kEmptyArr;
		}
	}

	public class All<C1> : Filter<C1>, IAll {}
	public class All<C1, C2> : Filter<C1, C2>, IAll { }
	public class All<C1, C2, C3> : Filter<C1, C2, C3>, IAll { }
	public class All<C1, C2, C3, C4> : Filter<C1, C2, C3, C4>, IAll { }
	public class All<C1, C2, C3, C4, C5> : Filter<C1, C2, C3, C4, C5>, IAll { }
	
	public class Any<C1> : Filter<C1>, IAny {}
	public class Any<C1, C2> : Filter<C1, C2>, IAny { }
	public class Any<C1, C2, C3> : Filter<C1, C2, C3>, IAny { }
	public class Any<C1, C2, C3, C4> : Filter<C1, C2, C3, C4>, IAny { }
	public class Any<C1, C2, C3, C4, C5> : Filter<C1, C2, C3, C4, C5>, IAny { }
	
	public class None<C1> : Filter<C1>, INone {}
	public class None<C1, C2> : Filter<C1, C2>, INone { }
	public class None<C1, C2, C3> : Filter<C1, C2, C3>, INone { }


	public interface IFilterBuilder
	{
		Filter Build(World world);
	}

	public class FilterBuilder<TAll> : IFilterBuilder where TAll : IAll, new()
	{
		public Filter Build(World world)
		{
			return new Filter(new TAll().GetIds(world), null, null);
		}
	}

	public class FilterBuilder<TAll, TAny> : IFilterBuilder where TAll : IAll, new() 
															where TAny : IAny, new()
	{
		public Filter Build(World world)
		{
			return new Filter(new TAll().GetIds(world), 
								new TAny().GetIds(world), 
								null);
		}
	}

	public class FilterBuilder<TAll, TAny, TNone> : IFilterBuilder where TAll : IAll, new()
																where TAny : IAny, new()
																where TNone : INone, new()
	{
		public Filter Build(World world)
		{
			return new Filter(new TAll().GetIds(world),
								new TAny().GetIds(world),
								new TNone().GetIds(world));
		}
	}
}