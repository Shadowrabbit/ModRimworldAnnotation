using System;

namespace RimWorld
{
	// Token: 0x020009F5 RID: 2549
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06003EC9 RID: 16073 RVA: 0x00157441 File Offset: 0x00155641
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x04002198 RID: 8600
		public float storedEnergyMax = 1000f;

		// Token: 0x04002199 RID: 8601
		public float efficiency = 0.5f;
	}
}
