using System;

namespace RimWorld
{
	// Token: 0x020013FE RID: 5118
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		// Token: 0x06007CEF RID: 31983 RVA: 0x002C430C File Offset: 0x002C250C
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		// Token: 0x04004514 RID: 17684
		public string defName;
	}
}
