using System;

namespace Verse
{
	// Token: 0x020000FE RID: 254
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x0000BD92 File Offset: 0x00009F92
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x04000439 RID: 1081
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x0400043A RID: 1082
		public int damagePerTickRare = 1;
	}
}
