using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA3 RID: 3747
	public class Thought_AttendedRitual : Thought_Memory
	{
		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005813 RID: 22547 RVA: 0x001DEBE4 File Offset: 0x001DCDE4
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005814 RID: 22548 RVA: 0x001DEC0B File Offset: 0x001DCE0B
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}
	}
}
