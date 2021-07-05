﻿using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015BE RID: 5566
	public class SymbolResolver_FirefoamPopper : SymbolResolver
	{
		// Token: 0x06008325 RID: 33573 RVA: 0x002EB488 File Offset: 0x002E9688
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x06008326 RID: 33574 RVA: 0x002EB4B4 File Offset: 0x002E96B4
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindSpawnCell(rp.rect, out loc))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.FirefoamPopper, null);
			thing.SetFaction(rp.faction, null);
			GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x06008327 RID: 33575 RVA: 0x002EB4FC File Offset: 0x002E96FC
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.Standable(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					return !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.FirefoamPopper, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				return false;
			}, out result);
		}
	}
}
