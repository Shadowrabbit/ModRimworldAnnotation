using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E0 RID: 736
	public class PawnCapacitiesHandler
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00070F3E File Offset: 0x0006F13E
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00070F55 File Offset: 0x0006F155
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00070F64 File Offset: 0x0006F164
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00070F70 File Offset: 0x0006F170
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
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
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

		// Token: 0x060013E7 RID: 5095 RVA: 0x0007101C File Offset: 0x0006F21C
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00071030 File Offset: 0x0006F230
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

		// Token: 0x04000E87 RID: 3719
		private Pawn pawn;

		// Token: 0x04000E88 RID: 3720
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02001A03 RID: 6659
		private enum CacheStatus
		{
			// Token: 0x040063BC RID: 25532
			Uncached,
			// Token: 0x040063BD RID: 25533
			Caching,
			// Token: 0x040063BE RID: 25534
			Cached
		}

		// Token: 0x02001A04 RID: 6660
		private class CacheElement
		{
			// Token: 0x040063BF RID: 25535
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x040063C0 RID: 25536
			public float value;
		}
	}
}
