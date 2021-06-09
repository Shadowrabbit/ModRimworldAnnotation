using System;

namespace RimWorld
{
	// Token: 0x02001C40 RID: 7232
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		// Token: 0x06009F41 RID: 40769 RVA: 0x0006A100 File Offset: 0x00068300
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		// Token: 0x04006573 RID: 25971
		public string modId;
	}
}
