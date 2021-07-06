using System;

namespace Verse.Noise
{
	// Token: 0x020008DF RID: 2271
	public class Displace : ModuleBase
	{
		// Token: 0x06003875 RID: 14453 RVA: 0x0002B9CB File Offset: 0x00029BCB
		public Displace() : base(4)
		{
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0002B9D4 File Offset: 0x00029BD4
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003877 RID: 14455 RVA: 0x0002BA02 File Offset: 0x00029C02
		// (set) Token: 0x06003878 RID: 14456 RVA: 0x0002BA0C File Offset: 0x00029C0C
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				this.modules[1] = value;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x0002B876 File Offset: 0x00029A76
		// (set) Token: 0x0600387A RID: 14458 RVA: 0x0002B880 File Offset: 0x00029A80
		public ModuleBase Y
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x0002BA17 File Offset: 0x00029C17
		// (set) Token: 0x0600387C RID: 14460 RVA: 0x0002BA21 File Offset: 0x00029C21
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				this.modules[3] = value;
			}
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x00163730 File Offset: 0x00161930
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
