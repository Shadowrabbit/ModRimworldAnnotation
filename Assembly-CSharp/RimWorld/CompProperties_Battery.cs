using System;

namespace RimWorld
{
	// Token: 0x02000F00 RID: 3840
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06005507 RID: 21767 RVA: 0x0003AF35 File Offset: 0x00039135
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x0400360B RID: 13835
		public float storedEnergyMax = 1000f;

		// Token: 0x0400360C RID: 13836
		public float efficiency = 0.5f;
	}
}
