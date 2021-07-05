using System;

namespace Verse
{
	// Token: 0x02000742 RID: 1858
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x06002EBD RID: 11965 RVA: 0x00024A3B File Offset: 0x00022C3B
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x04001FD1 RID: 8145
		public float lineHeight = 20f;
	}
}
