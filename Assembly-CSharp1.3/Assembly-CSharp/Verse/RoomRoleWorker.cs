using System;

namespace Verse
{
	// Token: 0x020000FD RID: 253
	public abstract class RoomRoleWorker
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public virtual string PostProcessedLabel(string baseLabel)
		{
			return baseLabel;
		}

		// Token: 0x060006D3 RID: 1747
		public abstract float GetScore(Room room);
	}
}
