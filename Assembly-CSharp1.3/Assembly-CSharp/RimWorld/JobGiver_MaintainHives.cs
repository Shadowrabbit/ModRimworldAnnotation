using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x001286A6 File Offset: 0x001268A6
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_MaintainHives jobGiver_MaintainHives = (JobGiver_MaintainHives)base.DeepCopy(resolve);
			jobGiver_MaintainHives.onlyIfDamagingState = this.onlyIfDamagingState;
			return jobGiver_MaintainHives;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x001286C0 File Offset: 0x001268C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			int num = 0;
			while ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map) == room)
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

		// Token: 0x04001E3F RID: 7743
		private bool onlyIfDamagingState;

		// Token: 0x04001E40 RID: 7744
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
