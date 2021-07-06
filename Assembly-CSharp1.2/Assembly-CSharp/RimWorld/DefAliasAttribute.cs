using System;

namespace RimWorld
{
	// Token: 0x02001C3F RID: 7231
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		// Token: 0x06009F40 RID: 40768 RVA: 0x0006A0F1 File Offset: 0x000682F1
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		// Token: 0x04006572 RID: 25970
		public string defName;
	}
}
