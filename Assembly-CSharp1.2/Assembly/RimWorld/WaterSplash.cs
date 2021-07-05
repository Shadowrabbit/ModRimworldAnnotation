using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001728 RID: 5928
	public class WaterSplash : Projectile
	{
		// Token: 0x060082BD RID: 33469 RVA: 0x0026C4BC File Offset: 0x0026A6BC
		protected override void Impact(Thing hitThing)
		{
			base.Impact(hitThing);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(base.Position))
			{
				if (thing.def == ThingDefOf.Fire)
				{
					list.Add(thing);
				}
			}
			foreach (Thing thing2 in list)
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
