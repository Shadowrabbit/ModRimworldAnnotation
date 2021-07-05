using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001739 RID: 5945
	public static class SkyfallerMaker
	{
		// Token: 0x06008329 RID: 33577 RVA: 0x00058101 File Offset: 0x00056301
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x0600832A RID: 33578 RVA: 0x0026E518 File Offset: 0x0026C718
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x0600832B RID: 33579 RVA: 0x0026E534 File Offset: 0x0026C734
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, Thing innerThing)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (innerThing != null && !skyfaller2.innerContainer.TryAdd(innerThing, true))
			{
				Log.Error("Could not add " + innerThing.ToStringSafe<Thing>() + " to a skyfaller.", false);
				innerThing.Destroy(DestroyMode.Vanish);
			}
			return skyfaller2;
		}

		// Token: 0x0600832C RID: 33580 RVA: 0x0026E580 File Offset: 0x0026C780
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x0600832D RID: 33581 RVA: 0x0005810F File Offset: 0x0005630F
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller), pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600832E RID: 33582 RVA: 0x00058124 File Offset: 0x00056324
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600832F RID: 33583 RVA: 0x0005813A File Offset: 0x0005633A
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06008330 RID: 33584 RVA: 0x00058150 File Offset: 0x00056350
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, things), pos, map, WipeMode.Vanish);
		}
	}
}
