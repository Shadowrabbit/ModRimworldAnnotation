using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D94 RID: 3476
	public class Recipe_AddHediff : Recipe_Surgery
	{
		// Token: 0x060050AC RID: 20652 RVA: 0x001AFEB4 File Offset: 0x001AE0B4
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn;
			return (pawn = (thing as Pawn)) != null && (part == null || (!pawn.health.WouldDieAfterAddingHediff(this.recipe.addsHediff, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(this.recipe.addsHediff, part, 1f))) && !pawn.health.hediffSet.HasHediff(this.recipe.addsHediff, false) && !pawn.BillStack.Bills.Any((Bill b) => b.recipe == this.recipe);
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001AFF50 File Offset: 0x001AE150
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			bool flag = this.IsViolationOnPawn(pawn, part, Faction.OfPlayerSilentFail);
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
			if (flag)
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
			}
		}
	}
}
