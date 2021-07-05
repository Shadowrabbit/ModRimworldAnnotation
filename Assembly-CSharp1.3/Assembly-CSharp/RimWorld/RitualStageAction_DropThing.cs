using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA1 RID: 4001
	public class RitualStageAction_DropThing : RitualStageAction
	{
		// Token: 0x06005E97 RID: 24215 RVA: 0x00206B3C File Offset: 0x00204D3C
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (Pawn pawn in ritual.assignments.Participants)
			{
				this.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00206B98 File Offset: 0x00204D98
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			Thing thing;
			if (carriedThing != null && carriedThing.def == this.def && carriedThing.stackCount >= this.count && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing, null))
			{
				return;
			}
			int num = Math.Min(pawn.inventory.Count(this.def), this.count);
			pawn.inventory.DropCount(this.def, num, false, false);
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x00206C19 File Offset: 0x00204E19
		public override void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x0400368A RID: 13962
		public ThingDef def;

		// Token: 0x0400368B RID: 13963
		public int count = 1;
	}
}
