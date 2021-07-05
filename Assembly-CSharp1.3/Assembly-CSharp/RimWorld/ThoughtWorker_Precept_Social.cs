using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000937 RID: 2359
	public abstract class ThoughtWorker_Precept_Social : ThoughtWorker
	{
		// Token: 0x06003CB8 RID: 15544
		protected abstract ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn);

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00150034 File Offset: 0x0014E234
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return false;
			}
			if (this.def.gender != Gender.None && otherPawn.gender != this.def.gender)
			{
				return false;
			}
			if (p.IsQuestLodger() || otherPawn.IsQuestLodger())
			{
				return false;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(this.def))
			{
				return false;
			}
			if (this.def.minExpectationForNegativeThought != null)
			{
				if (!p.Spawned)
				{
					return false;
				}
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (expectationDef != null && expectationDef.order < this.def.minExpectationForNegativeThought.order)
				{
					return false;
				}
			}
			return this.ShouldHaveThought(p, otherPawn);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x001500FC File Offset: 0x0014E2FC
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label) + " (" + "Ideo".Translate() + ")";
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00150130 File Offset: 0x0014E330
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "ComesFromIdeo".Translate() + ": " + p.Ideo.name;
		}
	}
}
