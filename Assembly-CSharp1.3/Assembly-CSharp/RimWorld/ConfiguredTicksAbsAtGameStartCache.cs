using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB5 RID: 3765
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x06005869 RID: 22633 RVA: 0x001E077B File Offset: 0x001DE97B
		public bool TryGetCachedValue(GameInitData initData, out int ticksAbs)
		{
			if (initData.startingTile == this.cachedForStartingTile && initData.startingSeason == this.cachedForStartingSeason)
			{
				ticksAbs = this.cachedTicks;
				return true;
			}
			ticksAbs = -1;
			return false;
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x001E07A7 File Offset: 0x001DE9A7
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x040033EB RID: 13291
		private int cachedTicks = -1;

		// Token: 0x040033EC RID: 13292
		private int cachedForStartingTile = -1;

		// Token: 0x040033ED RID: 13293
		private Season cachedForStartingSeason;
	}
}
