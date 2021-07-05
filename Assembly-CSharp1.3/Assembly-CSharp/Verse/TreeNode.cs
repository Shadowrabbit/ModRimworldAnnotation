using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000419 RID: 1049
	public class TreeNode
	{
		// Token: 0x06001F81 RID: 8065 RVA: 0x000C4312 File Offset: 0x000C2512
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000C431F File Offset: 0x000C251F
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001322 RID: 4898
		public TreeNode parentNode;

		// Token: 0x04001323 RID: 4899
		public List<TreeNode> children;

		// Token: 0x04001324 RID: 4900
		public int nestDepth;

		// Token: 0x04001325 RID: 4901
		private int openBits;
	}
}
