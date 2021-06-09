using System;

namespace Verse
{
	// Token: 0x02000797 RID: 1943
	public struct LabelValue
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x00026993 File Offset: 0x00024B93
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x0002699B File Offset: 0x00024B9B
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000269A3 File Offset: 0x00024BA3
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x00026993 File Offset: 0x00024B93
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x040021B4 RID: 8628
		private string label;

		// Token: 0x040021B5 RID: 8629
		private string value;
	}
}
