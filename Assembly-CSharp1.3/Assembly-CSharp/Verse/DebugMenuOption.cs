using System;

namespace Verse
{
	// Token: 0x020003C4 RID: 964
	public struct DebugMenuOption
	{
		// Token: 0x06001D9C RID: 7580 RVA: 0x000B92A2 File Offset: 0x000B74A2
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040011CF RID: 4559
		public DebugMenuOptionMode mode;

		// Token: 0x040011D0 RID: 4560
		public string label;

		// Token: 0x040011D1 RID: 4561
		public Action method;
	}
}
