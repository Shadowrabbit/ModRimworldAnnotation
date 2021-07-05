using System;

namespace Verse.Noise
{
	// Token: 0x02000530 RID: 1328
	public class Translate : ModuleBase
	{
		// Token: 0x060027FC RID: 10236 RVA: 0x000F3FF4 File Offset: 0x000F21F4
		public Translate() : base(1)
		{
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000F402A File Offset: 0x000F222A
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000F406C File Offset: 0x000F226C
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x000F40CC File Offset: 0x000F22CC
		// (set) Token: 0x06002800 RID: 10240 RVA: 0x000F40D4 File Offset: 0x000F22D4
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

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x000F40DD File Offset: 0x000F22DD
		// (set) Token: 0x06002802 RID: 10242 RVA: 0x000F40E5 File Offset: 0x000F22E5
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

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000F40EE File Offset: 0x000F22EE
		// (set) Token: 0x06002804 RID: 10244 RVA: 0x000F40F6 File Offset: 0x000F22F6
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

		// Token: 0x06002805 RID: 10245 RVA: 0x000F40FF File Offset: 0x000F22FF
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x040018A9 RID: 6313
		private double m_x = 1.0;

		// Token: 0x040018AA RID: 6314
		private double m_y = 1.0;

		// Token: 0x040018AB RID: 6315
		private double m_z = 1.0;
	}
}
