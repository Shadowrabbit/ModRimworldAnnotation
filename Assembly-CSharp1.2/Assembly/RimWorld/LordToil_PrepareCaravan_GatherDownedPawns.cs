using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE4 RID: 3556
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06005113 RID: 20755 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06005114 RID: 20756 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00038DD5 File Offset: 0x00036FD5
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001BA674 File Offset: 0x001B8874
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

		// Token: 0x06005117 RID: 20759 RVA: 0x001BA710 File Offset: 0x001B8910
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

		// Token: 0x04003426 RID: 13350
		private IntVec3 meetingPoint;

		// Token: 0x04003427 RID: 13351
		private IntVec3 exitSpot;
	}
}
