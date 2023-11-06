using System;
namespace ECS
{
	internal static class ArrayUtils
	{
		internal static void Ensure<T>(ref T[] arr, int capacity)
		{
			int oldCapacity = arr.Length;
			if (oldCapacity < capacity)
			{
				Array.Resize(ref arr, capacity);
			}
		}
	}
}
