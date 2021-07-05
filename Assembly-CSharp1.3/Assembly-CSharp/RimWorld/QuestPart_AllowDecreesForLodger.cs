using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B64 RID: 2916
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		// Token: 0x0600443A RID: 17466 RVA: 0x0016A4B7 File Offset: 0x001686B7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		// Token: 0x04002966 RID: 10598
		public Pawn lodger;
	}
}
