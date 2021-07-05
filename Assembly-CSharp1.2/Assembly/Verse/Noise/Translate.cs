using System;

namespace Verse.Noise
{
	// Token: 0x020008F1 RID: 2289
	public class Translate : ModuleBase
	{
		// Token: 0x060038DA RID: 14554 RVA: 0x0002BECD File Offset: 0x0002A0CD
		public Translate() : base(1)
		{
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0002BF03 File Offset: 0x0002A103
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x00163EA4 File Offset: 0x001620A4
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060038DD RID: 14557 RVA: 0x0002BF42 File Offset: 0x0002A142
		// (set) Token: 0x060038DE RID: 14558 RVA: 0x0002BF4A File Offset: 0x0002A14A
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

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060038DF RID: 14559 RVA: 0x0002BF53 File Offset: 0x0002A153
		// (set) Token: 0x060038E0 RID: 14560 RVA: 0x0002BF5B File Offset: 0x0002A15B
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

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060038E1 RID: 14561 RVA: 0x0002BF64 File Offset: 0x0002A164
		// (set) Token: 0x060038E2 RID: 14562 RVA: 0x0002BF6C File Offset: 0x0002A16C
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

		// Token: 0x060038E3 RID: 14563 RVA: 0x0002BF75 File Offset: 0x0002A175
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x04002724 RID: 10020
		private double m_x = 1.0;

		// Token: 0x04002725 RID: 10021
		private double m_y = 1.0;

		// Token: 0x04002726 RID: 10022
		private double m_z = 1.0;
	}
}
