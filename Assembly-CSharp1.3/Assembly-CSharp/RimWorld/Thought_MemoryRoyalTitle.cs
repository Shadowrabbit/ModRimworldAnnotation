using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9E RID: 3742
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x060057EF RID: 22511 RVA: 0x001DE488 File Offset: 0x001DC688
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x060057F0 RID: 22512 RVA: 0x001DE4D8 File Offset: 0x001DC6D8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x001DE525 File Offset: 0x001DC725
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		// Token: 0x040033D8 RID: 13272
		public RoyalTitleDef titleDef;
	}
}
