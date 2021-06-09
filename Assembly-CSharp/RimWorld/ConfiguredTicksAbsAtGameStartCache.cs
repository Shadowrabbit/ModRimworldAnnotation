using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001585 RID: 5509
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x06007771 RID: 30577 RVA: 0x000508E2 File Offset: 0x0004EAE2
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

		// Token: 0x06007772 RID: 30578 RVA: 0x0005090E File Offset: 0x0004EB0E
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x04004E9E RID: 20126
		private int cachedTicks = -1;

		// Token: 0x04004E9F RID: 20127
		private int cachedForStartingTile = -1;

		// Token: 0x04004EA0 RID: 20128
		private Season cachedForStartingSeason;
	}
}
