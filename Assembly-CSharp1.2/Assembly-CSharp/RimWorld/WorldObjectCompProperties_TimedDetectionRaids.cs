using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F60 RID: 3936
	public class WorldObjectCompProperties_TimedDetectionRaids : WorldObjectCompProperties
	{
		// Token: 0x06005681 RID: 22145 RVA: 0x0003C051 File Offset: 0x0003A251
		public WorldObjectCompProperties_TimedDetectionRaids()
		{
			this.compClass = typeof(TimedDetectionRaids);
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x0003C069 File Offset: 0x0003A269
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
