using System;

namespace Verse
{
	// Token: 0x02000087 RID: 135
	public class BodyPartGroupDef : Def
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00019E8D File Offset: 0x0001808D
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00019EA9 File Offset: 0x000180A9
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x040001DA RID: 474
		[MustTranslate]
		public string labelShort;

		// Token: 0x040001DB RID: 475
		public int listOrder;

		// Token: 0x040001DC RID: 476
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
