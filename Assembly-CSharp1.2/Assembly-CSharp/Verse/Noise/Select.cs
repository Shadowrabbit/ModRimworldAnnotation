using System;

namespace Verse.Noise
{
	// Token: 0x020008ED RID: 2285
	public class Select : ModuleBase
	{
		// Token: 0x060038BC RID: 14524 RVA: 0x0002BD65 File Offset: 0x00029F65
		public Select() : base(3)
		{
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x00163B08 File Offset: 0x00161D08
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0002BD8C File Offset: 0x00029F8C
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x060038BF RID: 14527 RVA: 0x0002B876 File Offset: 0x00029A76
		// (set) Token: 0x060038C0 RID: 14528 RVA: 0x0002B880 File Offset: 0x00029A80
		public ModuleBase Controller
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

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x060038C1 RID: 14529 RVA: 0x0002BDAE File Offset: 0x00029FAE
		// (set) Token: 0x060038C2 RID: 14530 RVA: 0x00163B58 File Offset: 0x00161D58
		public double FallOff
		{
			get
			{
				return this.m_fallOff;
			}
			set
			{
				double num = this.m_max - this.m_min;
				this.m_raw = value;
				this.m_fallOff = ((value > num / 2.0) ? (num / 2.0) : value);
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x060038C3 RID: 14531 RVA: 0x0002BDB6 File Offset: 0x00029FB6
		// (set) Token: 0x060038C4 RID: 14532 RVA: 0x0002BDBE File Offset: 0x00029FBE
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x060038C5 RID: 14533 RVA: 0x0002BDD3 File Offset: 0x00029FD3
		// (set) Token: 0x060038C6 RID: 14534 RVA: 0x0002BDDB File Offset: 0x00029FDB
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0002BDF0 File Offset: 0x00029FF0
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x00163B9C File Offset: 0x00161D9C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[2].GetValue(x, y, z);
			if (this.m_fallOff > 0.0)
			{
				if (value < this.m_min - this.m_fallOff)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				if (value < this.m_min + this.m_fallOff)
				{
					double num = this.m_min - this.m_fallOff;
					double num2 = this.m_min + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					return Utils.InterpolateLinear(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z), position);
				}
				if (value < this.m_max - this.m_fallOff)
				{
					return this.modules[1].GetValue(x, y, z);
				}
				if (value < this.m_max + this.m_fallOff)
				{
					double num3 = this.m_max - this.m_fallOff;
					double num4 = this.m_max + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					return Utils.InterpolateLinear(this.modules[1].GetValue(x, y, z), this.modules[0].GetValue(x, y, z), position);
				}
				return this.modules[0].GetValue(x, y, z);
			}
			else
			{
				if (value < this.m_min || value > this.m_max)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				return this.modules[1].GetValue(x, y, z);
			}
		}

		// Token: 0x0400271C RID: 10012
		private double m_fallOff;

		// Token: 0x0400271D RID: 10013
		private double m_raw;

		// Token: 0x0400271E RID: 10014
		private double m_min = -1.0;

		// Token: 0x0400271F RID: 10015
		private double m_max = 1.0;
	}
}
