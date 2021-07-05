using System;

namespace Verse.Noise
{
	// Token: 0x0200052D RID: 1325
	public class Select : ModuleBase
	{
		// Token: 0x060027E1 RID: 10209 RVA: 0x000F3B08 File Offset: 0x000F1D08
		public Select() : base(3)
		{
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000F3B30 File Offset: 0x000F1D30
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000F3B7D File Offset: 0x000F1D7D
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x000F2F68 File Offset: 0x000F1168
		// (set) Token: 0x060027E5 RID: 10213 RVA: 0x000F2F72 File Offset: 0x000F1172
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

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000F3B9F File Offset: 0x000F1D9F
		// (set) Token: 0x060027E7 RID: 10215 RVA: 0x000F3BA8 File Offset: 0x000F1DA8
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

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000F3BEC File Offset: 0x000F1DEC
		// (set) Token: 0x060027E9 RID: 10217 RVA: 0x000F3BF4 File Offset: 0x000F1DF4
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

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x000F3C09 File Offset: 0x000F1E09
		// (set) Token: 0x060027EB RID: 10219 RVA: 0x000F3C11 File Offset: 0x000F1E11
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

		// Token: 0x060027EC RID: 10220 RVA: 0x000F3C26 File Offset: 0x000F1E26
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000F3C44 File Offset: 0x000F1E44
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

		// Token: 0x040018A3 RID: 6307
		private double m_fallOff;

		// Token: 0x040018A4 RID: 6308
		private double m_raw;

		// Token: 0x040018A5 RID: 6309
		private double m_min = -1.0;

		// Token: 0x040018A6 RID: 6310
		private double m_max = 1.0;
	}
}
