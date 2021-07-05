using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9D RID: 2973
	public class QuestPart_RequirementsToAcceptPawnOnColonyMap : QuestPart_RequirementsToAccept
	{
		// Token: 0x06004573 RID: 17779 RVA: 0x001705E8 File Offset: 0x0016E7E8
		public override AcceptanceReport CanAccept()
		{
			if (this.pawn != null && this.pawn.Map != null && this.pawn.Map.IsPlayerHome)
			{
				return true;
			}
			return new AcceptanceReport("QuestPawnNotOnColonyMap".Translate(this.pawn.Named("PAWN")));
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00170647 File Offset: 0x0016E847
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004575 RID: 17781 RVA: 0x00170660 File Offset: 0x0016E860
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

		// Token: 0x04002A4E RID: 10830
		public Pawn pawn;
	}
}
