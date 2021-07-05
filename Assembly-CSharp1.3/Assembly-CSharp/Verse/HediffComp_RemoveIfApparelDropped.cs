using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B2 RID: 690
	public class HediffComp_RemoveIfApparelDropped : HediffComp
	{
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0006B97A File Offset: 0x00069B7A
		public HediffCompProperties_RemoveIfApparelDropped Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfApparelDropped)this.props;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0006B987 File Offset: 0x00069B87
		public override bool CompShouldRemove
		{
			get
			{
				return !this.parent.pawn.apparel.Wearing(this.wornApparel);
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0006B9A7 File Offset: 0x00069BA7
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Apparel>(ref this.wornApparel, "wornApparel", false);
		}

		// Token: 0x04000E29 RID: 3625
		public Apparel wornApparel;
	}
}
