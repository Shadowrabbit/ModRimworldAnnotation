using System;

namespace Verse.Noise
{
	// Token: 0x020008D0 RID: 2256
	public class Const : ModuleBase
	{
		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x0002B479 File Offset: 0x00029679
		// (set) Token: 0x060037FE RID: 14334 RVA: 0x0002B481 File Offset: 0x00029681
		public double Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0002B37B File Offset: 0x0002957B
		public Const() : base(0)
		{
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0002B48A File Offset: 0x0002968A
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x0002B479 File Offset: 0x00029679
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		// Token: 0x040026CF RID: 9935
		private double val;
	}
}
