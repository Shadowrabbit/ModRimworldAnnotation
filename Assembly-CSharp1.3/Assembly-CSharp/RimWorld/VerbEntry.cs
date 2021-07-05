using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6C RID: 3692
	public struct VerbEntry
	{
		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x060055C0 RID: 21952 RVA: 0x001D10C1 File Offset: 0x001CF2C1
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x001D10CE File Offset: 0x001CF2CE
		public VerbEntry(Verb verb, Pawn pawn)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn);
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x001D10EA File Offset: 0x001CF2EA
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x001D1103 File Offset: 0x001CF303
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			return this.cachedSelectionWeight;
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x001D111F File Offset: 0x001CF31F
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		// Token: 0x040032B5 RID: 12981
		public Verb verb;

		// Token: 0x040032B6 RID: 12982
		private float cachedSelectionWeight;
	}
}
