using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public class ThoughtWorker_WrongApparelGender : ThoughtWorker
	{
		// Token: 0x06003DD8 RID: 15832 RVA: 0x00153898 File Offset: 0x00151A98
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.Named("PAWN"));
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x001538CC File Offset: 0x00151ACC
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.gender.GetLabel(false), p.Named("PAWN"));
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0015391C File Offset: 0x00151B1C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!wornApparel[i].def.apparel.CorrectGenderForWearing(p.gender))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return false;
		}
	}
}
