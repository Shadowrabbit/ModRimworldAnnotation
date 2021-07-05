using System;

namespace Verse
{
	// Token: 0x0200009B RID: 155
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x0001A8FD File Offset: 0x00018AFD
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x04000266 RID: 614
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x04000267 RID: 615
		public int damagePerTickRare = 1;
	}
}
