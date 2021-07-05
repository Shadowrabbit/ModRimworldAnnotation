using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A6 RID: 4262
	public class TunnelJellySpawner : TunnelHiveSpawner
	{
		// Token: 0x0600659E RID: 26014 RVA: 0x0022558C File Offset: 0x0022378C
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.jellyCount > 0)
			{
				int num = Rand.Range(2, 3);
				int num2 = this.jellyCount;
				for (int i = 0; i < num; i++)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.InsectJelly, null);
					if (i < num - 1)
					{
						thing.stackCount = this.jellyCount / num;
						num2 -= thing.stackCount;
					}
					else
					{
						thing.stackCount = num2;
					}
					GenSpawn.Spawn(thing, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
				}
				this.jellyCount = 0;
			}
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x00225608 File Offset: 0x00223808
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jellyCount, "jellyCount", 0, false);
		}

		// Token: 0x0400395B RID: 14683
		public int jellyCount;
	}
}
