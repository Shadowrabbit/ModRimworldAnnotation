using System;

namespace Verse.Noise
{
	// Token: 0x0200051C RID: 1308
	public class Cache : ModuleBase
	{
		// Token: 0x06002785 RID: 10117 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public Cache() : base(1)
		{
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000F2E95 File Offset: 0x000F1095
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170007BE RID: 1982
		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000F2FF4 File Offset: 0x000F11F4
		public override double GetValue(double x, double y, double z)
		{
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}

		// Token: 0x04001885 RID: 6277
		private double m_value;

		// Token: 0x04001886 RID: 6278
		private bool m_cached;

		// Token: 0x04001887 RID: 6279
		private double m_x;

		// Token: 0x04001888 RID: 6280
		private double m_y;

		// Token: 0x04001889 RID: 6281
		private double m_z;
	}
}
