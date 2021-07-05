using System;

namespace Verse
{
	// Token: 0x0200044C RID: 1100
	public struct LabelValue
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x000D05D7 File Offset: 0x000CE7D7
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000D05DF File Offset: 0x000CE7DF
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000D05E7 File Offset: 0x000CE7E7
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000D05D7 File Offset: 0x000CE7D7
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x040014AF RID: 5295
		private string label;

		// Token: 0x040014B0 RID: 5296
		private string value;
	}
}
