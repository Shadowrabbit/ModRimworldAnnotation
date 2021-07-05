using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001105 RID: 4357
	public class QuestPart_RequirementsToAcceptPawnOnColonyMap : QuestPart_RequirementsToAccept
	{
		// Token: 0x06005F31 RID: 24369 RVA: 0x001E1D4C File Offset: 0x001DFF4C
		public override AcceptanceReport CanAccept()
		{
			if (this.pawn != null && this.pawn.Map != null && this.pawn.Map.IsPlayerHome)
			{
				return true;
			}
			return new AcceptanceReport("QuestPawnNotOnColonyMap".Translate(this.pawn.Named("PAWN")));
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x00041DCB File Offset: 0x0003FFCB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005F33 RID: 24371 RVA: 0x00041DE4 File Offset: 0x0003FFE4
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.pawn != null)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.pawn, -1);
				}
				yield break;
			}
		}

		// Token: 0x04003FB1 RID: 16305
		public Pawn pawn;
	}
}
