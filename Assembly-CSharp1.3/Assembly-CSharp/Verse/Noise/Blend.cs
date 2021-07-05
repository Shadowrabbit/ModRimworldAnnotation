using System;

namespace Verse.Noise
{
	// Token: 0x0200051B RID: 1307
	public class Blend : ModuleBase
	{
		// Token: 0x06002780 RID: 10112 RVA: 0x000F2F3B File Offset: 0x000F113B
		public Blend() : base(3)
		{
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000F2F44 File Offset: 0x000F1144
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x000F2F68 File Offset: 0x000F1168
		// (set) Token: 0x06002783 RID: 10115 RVA: 0x000F2F72 File Offset: 0x000F1172
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

		// Token: 0x06002784 RID: 10116 RVA: 0x000F2F80 File Offset: 0x000F1180
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
