using System;

namespace RimWorld.Planet
{
	// Token: 0x0200172B RID: 5931
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x060088C0 RID: 35008 RVA: 0x00312557 File Offset: 0x00310757
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x040056E2 RID: 22242
		public bool autoStartOnMapRemoved = true;

		// Token: 0x040056E3 RID: 22243
		public float durationDays = 1f;
	}
}
