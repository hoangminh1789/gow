namespace ECS
{
	public struct Constants
	{
		internal const int kPageMask = 0xFF;
		internal const int kPageSize = 4096; // 12 bits
		internal const int kPageShift = 12;
		internal const int kPositionMask = 0xFFFFF;
		internal const int kVersionMask = 0x7FF;
		internal const int kVersionShift = 20;
		public const int kNull = kPositionMask;
	}

	internal static class EntityExtensions
	{
		internal static int Version(this int entity)
		{
			return (entity >> Constants.kVersionShift) & Constants.kVersionMask;
		}

		internal static int Position(this int entity)
		{
			return entity & Constants.kPositionMask;
		}
	}
}