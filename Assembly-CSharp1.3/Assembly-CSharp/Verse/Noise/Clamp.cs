using System;

namespace Verse.Noise
{
	// Token: 0x0200051D RID: 1309
	public class Clamp : ModuleBase
	{
		// Token: 0x0600278A RID: 10122 RVA: 0x000F305C File Offset: 0x000F125C
		public Clamp() : base(1)
		{
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000F3083 File Offset: 0x000F1283
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000F30B3 File Offset: 0x000F12B3
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000F30F1 File Offset: 0x000F12F1
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x000F30F9 File Offset: 0x000F12F9
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000F3102 File Offset: 0x000F1302
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000F310A File Offset: 0x000F130A
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000F3113 File Offset: 0x000F1313
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000F3124 File Offset: 0x000F1324
		public override double GetValue(double x, double y, double z)
		{
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			if (value < this.m_min)
			{
				return this.m_min;
			}
			if (value > this.m_max)
			{
				return this.m_max;
			}
			return value;
		}

		// Token: 0x0400188A RID: 6282
		private double m_min = -1.0;

		// Token: 0x0400188B RID: 6283
		private double m_max = 1.0;
	}
}
