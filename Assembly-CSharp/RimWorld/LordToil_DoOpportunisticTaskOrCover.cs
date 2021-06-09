using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF5 RID: 3573
	public abstract class LordToil_DoOpportunisticTaskOrCover : LordToil
	{
		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x0600515A RID: 20826
		protected abstract DutyDef DutyDef { get; }

		// Token: 0x0600515B RID: 20827
		protected abstract bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets);

		// Token: 0x0600515C RID: 20828 RVA: 0x001BB1BC File Offset: 0x001B93BC
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

		// Token: 0x0600515D RID: 20829 RVA: 0x001BB28C File Offset: 0x001B948C
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

		// Token: 0x04003439 RID: 13369
		public bool cover = true;
	}
}
