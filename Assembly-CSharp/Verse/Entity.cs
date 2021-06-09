using System;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public abstract class Entity
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000C9C RID: 3228
		public abstract string LabelCap { get; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000C9D RID: 3229
		public abstract string Label { get; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0000FC1E File Offset: 0x0000DE1E
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0000FC1E File Offset: 0x0000DE1E
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0000FC26 File Offset: 0x0000DE26
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06000CA1 RID: 3233
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06000CA2 RID: 3234
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0000FC1E File Offset: 0x0000DE1E
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
