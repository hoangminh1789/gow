namespace ECS
{
	public partial class World
	{
		public delegate void F_C0<C1>(C1 c1);
		public delegate void F_C1<C1>(ref C1 c1);

		public delegate void F_CC0<C1, C2>(C1 c1, C2 c2);
		public delegate void F_CC1<C1, C2>(ref C1 c1, C2 c2);
		public delegate void F_CC2<C1, C2>(ref C1 c1, ref C2 c2);

		public delegate void F_CCC0<C1, C2, C3>(C1 c1, C2 c2, C3 c3);
		public delegate void F_CCC1<C1, C2, C3>(ref C1 c1, C2 c2, C3 c3);
		public delegate void F_CCC2<C1, C2, C3>(ref C1 c1, ref C2 c2, C3 c3);
		public delegate void F_CCC3<C1, C2, C3>(ref C1 c1, ref C2 c2, ref C3 c3);

		public delegate void F_EC0<C1>(int entity, C1 c1);
		public delegate void F_EC1<C1>(int entity, ref C1 c1);

		public delegate void F_ECC0<C1, C2>(int entity, C1 c1, C2 c2);
		public delegate void F_ECC1<C1, C2>(int entity, ref C1 c1, C2 c2);
		public delegate void F_ECC2<C1, C2>(int entity, ref C1 c1, ref C2 c2);

		public delegate void F_ECCC0<C1, C2, C3>(int entity, C1 c1, C2 c2, C3 c3);
		public delegate void F_ECCC1<C1, C2, C3>(int entity, ref C1 c1, C2 c2, C3 c3);
		public delegate void F_ECCC2<C1, C2, C3>(int entity, ref C1 c1, ref C2 c2, C3 c3);
		public delegate void F_ECCC3<C1, C2, C3>(int entity, ref C1 c1, ref C2 c2, ref C3 c3);

		#region C
		public void ForEach<C1>(F_C0<C1> action)
		{
			ForEach(All<C1>(), action);
		}

		public void ForEach<C1>(IEntityCollection collection, F_C0<C1> action)
		{
			var pool1 = Pool<C1>();
			var (entities, size) = collection.Raw();
			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(pool1.Get(entity));
			}
		}

		public void ForEach<C1>(F_C1<C1> action)
		{
			ForEach(All<C1>(), action);
		}

		public void ForEach<C1>(IEntityCollection collection, F_C1<C1> action)
		{
			var pool1 = Pool<C1>();
			var (entities, size) = collection.Raw();
			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity));
			}
			pool1.MarkChanged(entities, size);
		}
		#endregion

		#region EC
		public void ForEachEntity<C1>(F_EC0<C1> action)
		{
			ForEach(All<C1>(), action);
		}

		public void ForEach<C1>(IEntityCollection collection, F_EC0<C1> action)
		{
			var pool1 = Pool<C1>();
			var (entities, size) = collection.Raw();
			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(entity, pool1.Get(entity));
			}
		}

		public void ForEachEntity<C1>(F_EC1<C1> action)
		{
			ForEachEntity(All<C1>(), action);
		}

		public void ForEachEntity<C1>(IEntityCollection collection, F_EC1<C1> action)
		{
			var pool1 = Pool<C1>();
			var (entities, size) = collection.Raw();
			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(entity, ref pool1.GetRef(entity));
			}
			pool1.MarkChanged(entities, collection.Count);
		}
		#endregion


		#region CC
		public void ForEach<C1, C2>(F_CC0<C1, C2> action)
		{
			ForEach(All<C1, C2>(), action);
		}

		public void ForEach<C1, C2>(IEntityCollection collection, F_CC0<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = All<C1, C2>().Raw();
			
			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(pool1.Get(entity), pool2.Get(entity));
			}
		}

		public void ForEach<C1, C2>(F_CC1<C1, C2> action)
		{
			ForEach(All<C1, C2>(), action);
		}

		public void ForEach<C1, C2>(IEntityCollection collection, F_CC1<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = collection.Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity), pool2.Get(entity));
			}

			pool1.MarkChanged(entities, size);
		}

		public void ForEach<C1, C2>(F_CC2<C1, C2> action)
		{
			ForEach(All<C1, C2>(), action);
		}

		public void ForEach<C1, C2>(IEntityCollection collection, F_CC2<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = collection.Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity), ref pool2.GetRef(entity));
			}

			pool1.MarkChanged(entities, size);
			pool2.MarkChanged(entities, size);
		}
		#endregion

		#region ECC
		public void ForEachEntity<C1, C2>(F_ECC0<C1, C2> action)
		{
			ForEachEntity(All<C1, C2>(), action);
		}

		public void ForEachEntity<C1, C2>(IEntityCollection collection, F_ECC0<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = All<C1, C2>().Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(entity, pool1.Get(entity), pool2.Get(entity));
			}
		}

		public void ForEachEntity<C1, C2>(F_ECC1<C1, C2> action)
		{
			ForEachEntity(All<C1, C2>(), action);
		}

		public void ForEachEntity<C1, C2>(IEntityCollection collection, F_ECC1<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = collection.Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(entity, ref pool1.GetRef(entity), pool2.Get(entity));
			}

			pool1.MarkChanged(entities, size);
		}

		public void ForEachEntity<C1, C2>(F_ECC2<C1, C2> action)
		{
			ForEachEntity(All<C1, C2>(), action);
		}

		public void ForEachEntity<C1, C2>(IEntityCollection collection, F_ECC2<C1, C2> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var (entities, size) = collection.Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(entity, ref pool1.GetRef(entity), ref pool2.GetRef(entity));
			}

			pool1.MarkChanged(entities, size);
			pool2.MarkChanged(entities, size);
		}
		#endregion

		#region CCC
		public void ForEach<C1, C2, C3>(F_CCC0<C1, C2, C3> action)
		{
			ForEach(All<C1, C2, C3>(), action);
		}

		public void ForEach<C1, C2, C3>(IEntityCollection collection, F_CCC0<C1, C2, C3> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var pool3 = Pool<C3>();
			var (entities, size) = All<C1, C2, C3>().Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(pool1.Get(entity), pool2.Get(entity), pool3.Get(entity));
			}
		}

		public void ForEach<C1, C2, C3>(F_CCC1<C1, C2, C3> action)
		{
			ForEach(All<C1, C2, C3>(), action);
		}

		public void ForEach<C1, C2, C3>(IEntityCollection collection, F_CCC1<C1, C2, C3> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var pool3 = Pool<C3>();
			var (entities, size) = All<C1, C2, C3>().Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity), pool2.Get(entity), pool3.Get(entity));
			}

			pool1.MarkChanged(entities, size);
		}

		public void ForEach<C1, C2, C3>(F_CCC2<C1, C2, C3> action)
		{
			ForEach(All<C1, C2, C3>(), action);
		}

		public void ForEach<C1, C2, C3>(IEntityCollection collection, F_CCC2<C1, C2, C3> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var pool3 = Pool<C3>();
			var (entities, size) = All<C1, C2, C3>().Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity), ref pool2.GetRef(entity), pool3.Get(entity));
			}

			pool1.MarkChanged(entities, size);
			pool2.MarkChanged(entities, size);
		}

		public void ForEach<C1, C2, C3>(F_CCC3<C1, C2, C3> action)
		{
			ForEach(All<C1, C2, C3>(), action);
		}

		public void ForEach<C1, C2, C3>(IEntityCollection collection, F_CCC3<C1, C2, C3> action)
		{
			var pool1 = Pool<C1>();
			var pool2 = Pool<C2>();
			var pool3 = Pool<C3>();
			var (entities, size) = All<C1, C2, C3>().Raw();

			for (int i = size - 1; i >= 0; --i)
			{
				int entity = entities[i];
				action.Invoke(ref pool1.GetRef(entity), ref pool2.GetRef(entity), ref pool3.GetRef(entity));
			}

			pool1.MarkChanged(entities, size);
			pool2.MarkChanged(entities, size);
			pool3.MarkChanged(entities, size);
		}

		#endregion
	}
}