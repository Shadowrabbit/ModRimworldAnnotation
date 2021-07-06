using System;

namespace Verse
{
	// Token: 0x020006CC RID: 1740
	public struct DebugMenuOption
	{
		// Token: 0x06002CC5 RID: 11461 RVA: 0x0002378E File Offset: 0x0002198E
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x04001E54 RID: 7764
		public DebugMenuOptionMode mode;

		// Token: 0x04001E55 RID: 7765
		public string label;

		// Token: 0x04001E56 RID: 7766
		public Action method;
	}
}
