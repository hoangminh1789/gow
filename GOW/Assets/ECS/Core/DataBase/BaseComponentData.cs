using UnityEngine;

namespace ECS.DB
{
	public class BaseComponentData<T> : ScriptableObject
	{
		public int[] sparse;
		public int[] dense;
		[SerializeReference]
		public T[] components;
	}
}