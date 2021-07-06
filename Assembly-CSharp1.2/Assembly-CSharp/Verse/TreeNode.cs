using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200074E RID: 1870
	public class TreeNode
	{
		// Token: 0x06002F1C RID: 12060 RVA: 0x00024EF0 File Offset: 0x000230F0
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x00024EFD File Offset: 0x000230FD
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001FFA RID: 8186
		public TreeNode parentNode;

		// Token: 0x04001FFB RID: 8187
		public List<TreeNode> children;

		// Token: 0x04001FFC RID: 8188
		public int nestDepth;

		// Token: 0x04001FFD RID: 8189
		private int openBits;
	}
}
