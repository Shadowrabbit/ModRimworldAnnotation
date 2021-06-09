using System;

namespace Verse.Noise
{
	// Token: 0x020008DA RID: 2266
	public class Blend : ModuleBase
	{
		// Token: 0x06003858 RID: 14424 RVA: 0x0002B849 File Offset: 0x00029A49
		public Blend() : base(3)
		{
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0002B852 File Offset: 0x00029A52
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x0002B876 File Offset: 0x00029A76
		// (set) Token: 0x0600385B RID: 14427 RVA: 0x0002B880 File Offset: 0x00029A80
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

		// Token: 0x0600385C RID: 14428 RVA: 0x00163428 File Offset: 0x00161628
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
