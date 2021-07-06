using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001567 RID: 5479
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x060076D9 RID: 30425 RVA: 0x00242BA0 File Offset: 0x00240DA0
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x060076DA RID: 30426 RVA: 0x00242BF0 File Offset: 0x00240DF0
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x00050325 File Offset: 0x0004E525
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		// Token: 0x04004E65 RID: 20069
		public RoyalTitleDef titleDef;
	}
}
