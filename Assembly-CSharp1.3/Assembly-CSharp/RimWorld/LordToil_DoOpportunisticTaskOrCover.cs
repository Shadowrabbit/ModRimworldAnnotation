using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008AF RID: 2223
	public abstract class LordToil_DoOpportunisticTaskOrCover : LordToil
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003AC3 RID: 15043
		protected abstract DutyDef DutyDef { get; }

		// Token: 0x06003AC4 RID: 15044
		protected abstract bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets);

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00148960 File Offset: 0x00146B60
		public override void UpdateAllDuties()
		{
			List<Thing> list = null;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				Thing item = null;
				if (!this.cover || (this.TryFindGoodOpportunisticTaskTarget(pawn, out item, list) && !GenAI.InDangerousCombat(pawn)))
				{
					if (pawn.mindState.duty == null || pawn.mindState.duty.def != this.DutyDef)
					{
						pawn.mindState.duty = new PawnDuty(this.DutyDef);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					if (list == null)
					{
						list = new List<Thing>();
					}
					list.Add(item);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x00148A30 File Offset: 0x00146C30
		public override void LordToilTick()
		{
			if (this.cover && Find.TickManager.TicksGame % 181 == 0)
			{
				List<Thing> list = null;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Downed && pawn.mindState.duty.def == DutyDefOf.AssaultColony)
					{
						Thing thing = null;
						if (this.TryFindGoodOpportunisticTaskTarget(pawn, out thing, list) && !base.Map.reservationManager.IsReservedByAnyoneOf(thing, this.lord.faction) && !GenAI.InDangerousCombat(pawn))
						{
							pawn.mindState.duty = new PawnDuty(this.DutyDef);
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							if (list == null)
							{
								list = new List<Thing>();
							}
							list.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x0400201A RID: 8218
		public bool cover = true;
	}
}
