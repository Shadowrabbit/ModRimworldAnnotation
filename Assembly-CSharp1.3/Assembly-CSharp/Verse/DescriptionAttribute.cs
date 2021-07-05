using System;

namespace Verse
{
	// Token: 0x020003DB RID: 987
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06001DE3 RID: 7651 RVA: 0x000BAE59 File Offset: 0x000B9059
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x040011EE RID: 4590
		public string description;
	}
}
