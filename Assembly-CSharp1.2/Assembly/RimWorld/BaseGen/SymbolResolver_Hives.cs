using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E87 RID: 7815
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x0600A82F RID: 43055 RVA: 0x0030FC7C File Offset: 0x0030DE7C
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x0600A830 RID: 43056 RVA: 0x0030FCA8 File Offset: 0x0030DEA8
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindFirstHivePos(rp.rect, out loc))
			{
				return;
			}
			int num = rp.hivesCount ?? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange;
			Hive hive = (Hive)ThingMaker.MakeThing(ThingDefOf.Hive, null);
			hive.SetFaction(Faction.OfInsects, null);
			if (rp.disableHives != null && rp.disableHives.Value)
			{
				hive.CompDormant.ToSleep();
			}
			hive = (Hive)GenSpawn.Spawn(hive, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
			for (int i = 0; i < num - 1; i++)
			{
				Hive hive2;
				if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(true, out hive2))
				{
					hive = hive2;
				}
			}
		}

		// Token: 0x0600A831 RID: 43057 RVA: 0x0030FD70 File Offset: 0x0030DF70
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x0400721E RID: 29214
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
