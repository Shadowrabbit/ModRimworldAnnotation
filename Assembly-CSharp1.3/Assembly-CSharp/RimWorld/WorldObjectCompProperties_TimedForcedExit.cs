using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A40 RID: 2624
	[Obsolete]
	public class WorldObjectCompProperties_TimedForcedExit : WorldObjectCompProperties
	{
		// Token: 0x06003F6F RID: 16239 RVA: 0x00158D4A File Offset: 0x00156F4A
		public WorldObjectCompProperties_TimedForcedExit()
		{
			this.compClass = typeof(TimedForcedExit);
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00158D62 File Offset: 0x00156F62
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedForcedExit but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
