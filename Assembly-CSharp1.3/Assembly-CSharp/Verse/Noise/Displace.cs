using System;

namespace Verse.Noise
{
	// Token: 0x0200051F RID: 1311
	public class Displace : ModuleBase
	{
		// Token: 0x0600279A RID: 10138 RVA: 0x000F3397 File Offset: 0x000F1597
		public Displace() : base(4)
		{
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000F33A0 File Offset: 0x000F15A0
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000F33CE File Offset: 0x000F15CE
		// (set) Token: 0x0600279D RID: 10141 RVA: 0x000F33D8 File Offset: 0x000F15D8
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				this.modules[1] = value;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000F2F68 File Offset: 0x000F1168
		// (set) Token: 0x0600279F RID: 10143 RVA: 0x000F2F72 File Offset: 0x000F1172
		public ModuleBase Y
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

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000F33E3 File Offset: 0x000F15E3
		// (set) Token: 0x060027A1 RID: 10145 RVA: 0x000F33ED File Offset: 0x000F15ED
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				this.modules[3] = value;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000F33F8 File Offset: 0x000F15F8
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
