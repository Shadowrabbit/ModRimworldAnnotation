using System;

namespace Verse.AI
{
	// Token: 0x020005FF RID: 1535
	public class PathingContext
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x00106FD8 File Offset: 0x001051D8
		public PathingContext(Map map, PathGrid pathGrid)
		{
			this.map = map;
			this.pathGrid = pathGrid;
		}

		// Token: 0x04001ADB RID: 6875
		public readonly Map map;

		// Token: 0x04001ADC RID: 6876
		public readonly PathGrid pathGrid;
	}
}
