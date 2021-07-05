using System;

namespace RimWorld.Planet
{
	// Token: 0x02002007 RID: 8199
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x0600ADAA RID: 44458 RVA: 0x00071029 File Offset: 0x0006F229
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x0400774C RID: 30540
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400774D RID: 30541
		public float durationDays = 1f;
	}
}
