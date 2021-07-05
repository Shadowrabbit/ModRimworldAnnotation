using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A3 RID: 4259
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		// Token: 0x06005CE5 RID: 23781 RVA: 0x00040728 File Offset: 0x0003E928
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		// Token: 0x04003E33 RID: 15923
		public Pawn lodger;
	}
}
