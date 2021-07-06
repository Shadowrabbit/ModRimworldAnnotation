using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C71 RID: 3185
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		// Token: 0x06004AA2 RID: 19106 RVA: 0x0003571F File Offset: 0x0003391F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_MaintainHives jobGiver_MaintainHives = (JobGiver_MaintainHives)base.DeepCopy(resolve);
			jobGiver_MaintainHives.onlyIfDamagingState = this.onlyIfDamagingState;
			return jobGiver_MaintainHives;
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x001A24C0 File Offset: 0x001A06C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			int num = 0;
			while ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map, RegionType.Set_Passable) == room)
				{
					Hive hive = (Hive)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.Hive);
					if (hive != null && pawn.CanReserve(hive, 1, -1, null, false))
					{
						CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
						if (compMaintainable.CurStage != MaintainableStage.Healthy && (!this.onlyIfDamagingState || compMaintainable.CurStage == MaintainableStage.Damaging))
						{
							return JobMaker.MakeJob(JobDefOf.Maintain, hive);
						}
					}
				}
				num++;
			}
			return null;
		}

		// Token: 0x04003180 RID: 12672
		private bool onlyIfDamagingState;

		// Token: 0x04003181 RID: 12673
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
