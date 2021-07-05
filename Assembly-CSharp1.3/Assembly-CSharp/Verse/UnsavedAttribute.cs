using System;

namespace Verse
{
	// Token: 0x020003D3 RID: 979
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x06001DD9 RID: 7641 RVA: 0x000BAD6D File Offset: 0x000B8F6D
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x040011EB RID: 4587
		public bool allowLoading;
	}
}
