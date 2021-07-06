using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DEF RID: 3567
	public class LordToil_AssaultThings : LordToil
	{
		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00038F00 File Offset: 0x00037100
		public LordToil_AssaultThings(IEnumerable<Thing> things)
		{
			this.things = new List<Thing>(things);
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00038F14 File Offset: 0x00037114
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.UpdateAllDuties();
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001BAE40 File Offset: 0x001B9040
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = this.lord.ownedPawns[i].mindState.duty;
				if (duty == null || duty.def != DutyDefOf.AssaultThing || duty.focus.ThingDestroyed)
				{
					Thing t2;
					if (!(from t in this.things
					where t != null && t.Spawned
					select t).TryRandomElement(out t2))
					{
						break;
					}
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultThing, t2, -1f);
				}
			}
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x00038F1C File Offset: 0x0003711C
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x04003432 RID: 13362
		private List<Thing> things;

		// Token: 0x04003433 RID: 13363
		public const int UpdateIntervalTicks = 300;
	}
}
