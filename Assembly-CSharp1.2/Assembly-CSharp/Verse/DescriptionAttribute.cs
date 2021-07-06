using System;

namespace Verse
{
	// Token: 0x020006EB RID: 1771
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06002D19 RID: 11545 RVA: 0x00023A1D File Offset: 0x00021C1D
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x04001E85 RID: 7813
		public string description;
	}
}
