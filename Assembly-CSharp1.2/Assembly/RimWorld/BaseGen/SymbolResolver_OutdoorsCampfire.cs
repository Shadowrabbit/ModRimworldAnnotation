using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E60 RID: 7776
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		// Token: 0x0600A7BD RID: 42941 RVA: 0x0030D33C File Offset: 0x0030B53C
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x0600A7BE RID: 42942 RVA: 0x0030D368 File Offset: 0x0030B568
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindSpawnCell(rp.rect, out loc))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Campfire, null);
			thing.SetFaction(rp.faction, null);
			GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x0600A7BF RID: 42943 RVA: 0x0030D3B0 File Offset: 0x0030B5B0
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.Standable(map) && !c.Roofed(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					return !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.Campfire, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				return false;
			}, out result);
		}
	}
}
