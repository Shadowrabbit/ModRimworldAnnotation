using System;

namespace Verse
{
	// Token: 0x020000E5 RID: 229
	public class BodyPartGroupDef : Def
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0000B971 File Offset: 0x00009B71
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0000B98D File Offset: 0x00009B8D
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return base.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x040003A0 RID: 928
		[MustTranslate]
		public string labelShort;

		// Token: 0x040003A1 RID: 929
		public int listOrder;

		// Token: 0x040003A2 RID: 930
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
