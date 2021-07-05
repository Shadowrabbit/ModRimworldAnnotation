using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	public class Thought_RelicAtRitual : Thought_Memory
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x001515AC File Offset: 0x0014F7AC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003D2B RID: 15659 RVA: 0x001515E8 File Offset: 0x0014F7E8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00151622 File Offset: 0x0014F822
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.relicName, "relicName", null, false);
		}

		// Token: 0x040020CE RID: 8398
		public string relicName;
	}
}
