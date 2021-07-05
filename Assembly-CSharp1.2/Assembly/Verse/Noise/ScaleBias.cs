using System;

namespace Verse.Noise
{
	// Token: 0x020008EC RID: 2284
	public class ScaleBias : ModuleBase
	{
		// Token: 0x060038B4 RID: 14516 RVA: 0x0002BCBB File Offset: 0x00029EBB
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0002BCD3 File Offset: 0x00029ED3
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0002BCF4 File Offset: 0x00029EF4
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x0002BD23 File Offset: 0x00029F23
		// (set) Token: 0x060038B8 RID: 14520 RVA: 0x0002BD2B File Offset: 0x00029F2B
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x060038B9 RID: 14521 RVA: 0x0002BD34 File Offset: 0x00029F34
		// (set) Token: 0x060038BA RID: 14522 RVA: 0x0002BD3C File Offset: 0x00029F3C
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0002BD45 File Offset: 0x00029F45
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x0400271A RID: 10010
		private double scale = 1.0;

		// Token: 0x0400271B RID: 10011
		private double bias;
	}
}
