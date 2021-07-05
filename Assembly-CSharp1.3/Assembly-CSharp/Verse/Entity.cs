using System;

namespace Verse
{
	// Token: 0x02000140 RID: 320
	public abstract class Entity
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008E0 RID: 2272
		public abstract string LabelCap { get; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008E1 RID: 2273
		public abstract string Label { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00029737 File Offset: 0x00027937
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00029737 File Offset: 0x00027937
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0002973F File Offset: 0x0002793F
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x060008E5 RID: 2277
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x060008E6 RID: 2278
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00029737 File Offset: 0x00027937
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
