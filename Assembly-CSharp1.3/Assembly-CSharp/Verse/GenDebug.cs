using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B1 RID: 1201
	public static class GenDebug
	{
		// Token: 0x06002498 RID: 9368 RVA: 0x000E3818 File Offset: 0x000E1A18
		public static void DebugPlaceSphere(Vector3 Loc, float Scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = Loc;
			gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000E3840 File Offset: 0x000E1A40
		public static void LogList<T>(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Log.Message("    " + t.ToString());
			}
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000E38A0 File Offset: 0x000E1AA0
		public static void ClearArea(CellRect r, Map map)
		{
			r.ClipInsideMap(map);
			foreach (IntVec3 c in r)
			{
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 c2 in r)
			{
				foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.destroyable)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}
	}
}
