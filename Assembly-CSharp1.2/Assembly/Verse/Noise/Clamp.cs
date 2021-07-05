using System;

namespace Verse.Noise
{
	// Token: 0x020008DC RID: 2268
	public class Clamp : ModuleBase
	{
		// Token: 0x06003862 RID: 14434 RVA: 0x0002B8A5 File Offset: 0x00029AA5
		public Clamp() : base(1)
		{
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x0002B8CC File Offset: 0x00029ACC
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0002B8FC File Offset: 0x00029AFC
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x0002B93A File Offset: 0x00029B3A
		// (set) Token: 0x06003866 RID: 14438 RVA: 0x0002B942 File Offset: 0x00029B42
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

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003867 RID: 14439 RVA: 0x0002B94B File Offset: 0x00029B4B
		// (set) Token: 0x06003868 RID: 14440 RVA: 0x0002B953 File Offset: 0x00029B53
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

		// Token: 0x06003869 RID: 14441 RVA: 0x0002B95C File Offset: 0x00029B5C
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x001634EC File Offset: 0x001616EC
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

		// Token: 0x04002701 RID: 9985
		private double m_min = -1.0;

		// Token: 0x04002702 RID: 9986
		private double m_max = 1.0;
	}
}
