using System;

namespace RimWorld
{
	// Token: 0x020013FF RID: 5119
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		// Token: 0x06007CF0 RID: 31984 RVA: 0x002C431B File Offset: 0x002C251B
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		// Token: 0x04004515 RID: 17685
		public string modId;
	}
}
