using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C2 RID: 5570
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		// Token: 0x06008330 RID: 33584 RVA: 0x002EB924 File Offset: 0x002E9B24
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x06008331 RID: 33585 RVA: 0x002EB950 File Offset: 0x002E9B50
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

		// Token: 0x06008332 RID: 33586 RVA: 0x002EB998 File Offset: 0x002E9B98
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
