using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008AA RID: 2218
	public class LordToil_AssaultThings : LordToil
	{
		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00148585 File Offset: 0x00146785
		public LordToil_AssaultThings(IEnumerable<Thing> things)
		{
			this.things = new List<Thing>(things);
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00148599 File Offset: 0x00146799
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.UpdateAllDuties();
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x001485A4 File Offset: 0x001467A4
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

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00147A6F File Offset: 0x00145C6F
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x04002015 RID: 8213
		private List<Thing> things;

		// Token: 0x04002016 RID: 8214
		public const int UpdateIntervalTicks = 300;
	}
}
