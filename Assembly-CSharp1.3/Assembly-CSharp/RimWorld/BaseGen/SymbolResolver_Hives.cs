using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E1 RID: 5601
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x06008392 RID: 33682 RVA: 0x002F013C File Offset: 0x002EE33C
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && Faction.OfInsects != null && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x002F0170 File Offset: 0x002EE370
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

		// Token: 0x06008394 RID: 33684 RVA: 0x002F0238 File Offset: 0x002EE438
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x04005228 RID: 21032
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
