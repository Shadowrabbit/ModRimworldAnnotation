using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A3F RID: 2623
	public class WorldObjectCompProperties_TimedDetectionRaids : WorldObjectCompProperties
	{
		// Token: 0x06003F6C RID: 16236 RVA: 0x00158D1B File Offset: 0x00156F1B
		public WorldObjectCompProperties_TimedDetectionRaids()
		{
			this.compClass = typeof(TimedDetectionRaids);
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x00158D33 File Offset: 0x00156F33
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedDetectionRaids but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
