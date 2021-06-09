using System;

namespace Verse.Noise
{
	// Token: 0x020008EB RID: 2283
	public class Scale : ModuleBase
	{
		// Token: 0x060038AA RID: 14506 RVA: 0x0002BBEC File Offset: 0x00029DEC
		public Scale() : base(1)
		{
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x0002BC22 File Offset: 0x00029E22
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x00163AA8 File Offset: 0x00161CA8
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x0002BC61 File Offset: 0x00029E61
		// (set) Token: 0x060038AE RID: 14510 RVA: 0x0002BC69 File Offset: 0x00029E69
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

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060038AF RID: 14511 RVA: 0x0002BC72 File Offset: 0x00029E72
		// (set) Token: 0x060038B0 RID: 14512 RVA: 0x0002BC7A File Offset: 0x00029E7A
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

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x0002BC83 File Offset: 0x00029E83
		// (set) Token: 0x060038B2 RID: 14514 RVA: 0x0002BC8B File Offset: 0x00029E8B
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

		// Token: 0x060038B3 RID: 14515 RVA: 0x0002BC94 File Offset: 0x00029E94
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x04002717 RID: 10007
		private double m_x = 1.0;

		// Token: 0x04002718 RID: 10008
		private double m_y = 1.0;

		// Token: 0x04002719 RID: 10009
		private double m_z = 1.0;
	}
}
