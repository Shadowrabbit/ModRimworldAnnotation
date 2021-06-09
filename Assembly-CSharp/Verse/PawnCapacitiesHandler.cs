using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000425 RID: 1061
	public class PawnCapacitiesHandler
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x00018191 File Offset: 0x00016391
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x000181A8 File Offset: 0x000163A8
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x000181B7 File Offset: 0x000163B7
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x000E3C04 File Offset: 0x000E1E04
		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity), false);
				return 0f;
			}
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
			{
				cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
				try
				{
					cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null, false);
				}
				finally
				{
					cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
				}
			}
			return cacheElement.value;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000181C0 File Offset: 0x000163C0
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000E3CB4 File Offset: 0x000E1EB4
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x0400133D RID: 4925
		private Pawn pawn;

		// Token: 0x0400133E RID: 4926
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02000426 RID: 1062
		private enum CacheStatus
		{
			// Token: 0x04001340 RID: 4928
			Uncached,
			// Token: 0x04001341 RID: 4929
			Caching,
			// Token: 0x04001342 RID: 4930
			Cached
		}

		// Token: 0x02000427 RID: 1063
		private class CacheElement
		{
			// Token: 0x04001343 RID: 4931
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04001344 RID: 4932
			public float value;
		}
	}
}
