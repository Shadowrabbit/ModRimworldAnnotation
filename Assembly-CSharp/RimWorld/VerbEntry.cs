using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001511 RID: 5393
	public struct VerbEntry
	{
		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06007448 RID: 29768 RVA: 0x0004E676 File Offset: 0x0004C876
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x06007449 RID: 29769 RVA: 0x0004E683 File Offset: 0x0004C883
		public VerbEntry(Verb verb, Pawn pawn)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn);
		}

		// Token: 0x0600744A RID: 29770 RVA: 0x0004E69F File Offset: 0x0004C89F
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		// Token: 0x0600744B RID: 29771 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			return this.cachedSelectionWeight;
		}

		// Token: 0x0600744C RID: 29772 RVA: 0x0004E6D4 File Offset: 0x0004C8D4
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		// Token: 0x04004CBA RID: 19642
		public Verb verb;

		// Token: 0x04004CBB RID: 19643
		private float cachedSelectionWeight;
	}
}
