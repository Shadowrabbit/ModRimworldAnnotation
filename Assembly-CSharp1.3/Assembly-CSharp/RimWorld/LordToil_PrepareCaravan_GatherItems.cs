using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200089A RID: 2202
	public class LordToil_PrepareCaravan_GatherItems : LordToil
	{
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00147048 File Offset: 0x00145248
		public LordToil_PrepareCaravan_GatherItems(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x00147058 File Offset: 0x00145258
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherItems);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x001470D8 File Offset: 0x001452D8
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (Find.TickManager.TicksGame % 120 == 0)
			{
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.IsColonist && pawn.mindState.lastJobTag != JobTag.WaitingForOthersToFinishGatheringItems)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					List<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						if (allPawnsSpawned[j].CurJob != null && allPawnsSpawned[j].jobs.curDriver is JobDriver_PrepareCaravan_GatherItems && allPawnsSpawned[j].CurJob.lord == this.lord)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllItemsGathered");
				}
			}
		}

		// Token: 0x04001FF7 RID: 8183
		private IntVec3 meetingPoint;
	}
}
