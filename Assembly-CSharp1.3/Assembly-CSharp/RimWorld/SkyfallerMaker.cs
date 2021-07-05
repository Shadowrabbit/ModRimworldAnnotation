using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D3 RID: 4307
	public static class SkyfallerMaker
	{
		// Token: 0x0600671E RID: 26398 RVA: 0x0022DA38 File Offset: 0x0022BC38
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x0022DA48 File Offset: 0x0022BC48
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x0022DA64 File Offset: 0x0022BC64
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, Thing innerThing)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (innerThing != null && !skyfaller2.innerContainer.TryAdd(innerThing, true))
			{
				Log.Error("Could not add " + innerThing.ToStringSafe<Thing>() + " to a skyfaller.");
				innerThing.Destroy(DestroyMode.Vanish);
			}
			return skyfaller2;
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x0022DAAC File Offset: 0x0022BCAC
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x0022DAD2 File Offset: 0x0022BCD2
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06006723 RID: 26403 RVA: 0x0022DAE7 File Offset: 0x0022BCE7
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x0022DAFD File Offset: 0x0022BCFD
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x0022DB13 File Offset: 0x0022BD13
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, things), pos, map, WipeMode.Vanish);
		}
	}
}
