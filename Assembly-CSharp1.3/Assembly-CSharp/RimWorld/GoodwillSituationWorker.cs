using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC3 RID: 3779
	public abstract class GoodwillSituationWorker
	{
		// Token: 0x06005935 RID: 22837 RVA: 0x001E6FB3 File Offset: 0x001E51B3
		public virtual string GetPostProcessedLabel(Faction other)
		{
			return this.def.label;
		}

		// Token: 0x06005936 RID: 22838 RVA: 0x001E6FC0 File Offset: 0x001E51C0
		public string GetPostProcessedLabelCap(Faction other)
		{
			return this.GetPostProcessedLabel(other).CapitalizeFirst(this.def);
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x001E6FD4 File Offset: 0x001E51D4
		public virtual int GetMaxGoodwill(Faction other)
		{
			return 100;
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual int GetNaturalGoodwillOffset(Faction other)
		{
			return 0;
		}

		// Token: 0x04003460 RID: 13408
		public GoodwillSituationDef def;
	}
}
