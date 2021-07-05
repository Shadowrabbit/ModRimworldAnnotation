using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000872 RID: 2162
	public class LordToil_BestowingCeremony_Perform : LordToil_Wait
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x0013F411 File Offset: 0x0013D611
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0013F41E File Offset: 0x0013D61E
		public LordToil_BestowingCeremony_Perform(Pawn target, Pawn bestower)
		{
			this.target = target;
			this.bestower = bestower;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0013F440 File Offset: 0x0013D640
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.target.Position, base.Map))
				{
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0013F4E8 File Offset: 0x0013D6E8
		public override void UpdateAllDuties()
		{
			IntVec3 spot = ((LordJob_BestowingCeremony)this.lord.LordJob).GetSpot();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.Awake())
				{
					RestUtility.WakeUp(pawn);
				}
				if (pawn == this.bestower)
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Idle);
					pawnDuty.focus = spot;
					pawn.mindState.duty = pawnDuty;
				}
				else if (pawn == this.target)
				{
					PawnDuty duty = new PawnDuty(DutyDefOf.Bestow, this.bestower, spot, -1f);
					pawn.mindState.duty = duty;
				}
				else
				{
					PawnDuty pawnDuty2 = new PawnDuty(DutyDefOf.Spectate, spot, -1f);
					pawnDuty2.spectateRect = CellRect.CenteredOn(spot, 0);
					pawnDuty2.spectateRectAllowedSides = SpectateRectSide.All;
					pawnDuty2.spectateDistance = new IntRange(2, 2);
					pawn.mindState.duty = pawnDuty2;
				}
			}
		}

		// Token: 0x04001F54 RID: 8020
		public Pawn target;

		// Token: 0x04001F55 RID: 8021
		public Pawn bestower;
	}
}
