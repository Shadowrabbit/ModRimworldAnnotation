using System;

namespace Verse.Noise
{
	// Token: 0x02000511 RID: 1297
	public class Const : ModuleBase
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x000F19B6 File Offset: 0x000EFBB6
		// (set) Token: 0x06002726 RID: 10022 RVA: 0x000F19BE File Offset: 0x000EFBBE
		public double Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000F177C File Offset: 0x000EF97C
		public Const() : base(0)
		{
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000F19C7 File Offset: 0x000EFBC7
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000F19B6 File Offset: 0x000EFBB6
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		// Token: 0x04001858 RID: 6232
		private double val;
	}
}
