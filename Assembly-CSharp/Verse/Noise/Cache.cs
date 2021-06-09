using System;

namespace Verse.Noise
{
	// Token: 0x020008DB RID: 2267
	public class Cache : ModuleBase
	{
		// Token: 0x0600385D RID: 14429 RVA: 0x0002B39E File Offset: 0x0002959E
		public Cache() : base(1)
		{
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x0002B7A3 File Offset: 0x000299A3
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170008E6 RID: 2278
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

		// Token: 0x06003861 RID: 14433 RVA: 0x00163484 File Offset: 0x00161684
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

		// Token: 0x040026FC RID: 9980
		private double m_value;

		// Token: 0x040026FD RID: 9981
		private bool m_cached;

		// Token: 0x040026FE RID: 9982
		private double m_x;

		// Token: 0x040026FF RID: 9983
		private double m_y;

		// Token: 0x04002700 RID: 9984
		private double m_z;
	}
}
