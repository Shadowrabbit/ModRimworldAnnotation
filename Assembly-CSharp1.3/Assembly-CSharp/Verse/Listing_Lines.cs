using System;

namespace Verse
{
	// Token: 0x02000410 RID: 1040
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x06001F31 RID: 7985 RVA: 0x000C276D File Offset: 0x000C096D
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x04001300 RID: 4864
		public float lineHeight = 20f;
	}
}
