using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000899 RID: 2201
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00146F23 File Offset: 0x00145123
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00146F3C File Offset: 0x0014513C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherDownedPawns, this.meetingPoint, this.exitSpot, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00146FD8 File Offset: 0x001451D8
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				bool flag = true;
				List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)this.lord.LordJob).downedPawns;
				for (int i = 0; i < downedPawns.Count; i++)
				{
					if (!JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(downedPawns[i], this.exitSpot))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllDownedPawnsGathered");
				}
			}
		}

		// Token: 0x04001FF5 RID: 8181
		private IntVec3 meetingPoint;

		// Token: 0x04001FF6 RID: 8182
		private IntVec3 exitSpot;
	}
}
