using System;

namespace Verse.Noise
{
	// Token: 0x0200052B RID: 1323
	public class Scale : ModuleBase
	{
		// Token: 0x060027CF RID: 10191 RVA: 0x000F392E File Offset: 0x000F1B2E
		public Scale() : base(1)
		{
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000F3964 File Offset: 0x000F1B64
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000F39A4 File Offset: 0x000F1BA4
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060027D2 RID: 10194 RVA: 0x000F3A04 File Offset: 0x000F1C04
		// (set) Token: 0x060027D3 RID: 10195 RVA: 0x000F3A0C File Offset: 0x000F1C0C
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060027D4 RID: 10196 RVA: 0x000F3A15 File Offset: 0x000F1C15
		// (set) Token: 0x060027D5 RID: 10197 RVA: 0x000F3A1D File Offset: 0x000F1C1D
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060027D6 RID: 10198 RVA: 0x000F3A26 File Offset: 0x000F1C26
		// (set) Token: 0x060027D7 RID: 10199 RVA: 0x000F3A2E File Offset: 0x000F1C2E
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000F3A37 File Offset: 0x000F1C37
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x0400189E RID: 6302
		private double m_x = 1.0;

		// Token: 0x0400189F RID: 6303
		private double m_y = 1.0;

		// Token: 0x040018A0 RID: 6304
		private double m_z = 1.0;
	}
}
