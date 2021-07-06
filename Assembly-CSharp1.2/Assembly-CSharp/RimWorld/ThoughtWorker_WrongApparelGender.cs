using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB0 RID: 3760
	public class ThoughtWorker_WrongApparelGender : ThoughtWorker
	{
		// Token: 0x060053AF RID: 21423 RVA: 0x0003A413 File Offset: 0x00038613
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.Named("PAWN"));
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x001C17B4 File Offset: 0x001BF9B4
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.gender.GetLabel(false), p.Named("PAWN"));
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x001C1804 File Offset: 0x001BFA04
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
