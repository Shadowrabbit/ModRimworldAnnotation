using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A41 RID: 2625
	public class WorldObjectCompProperties_TimedMakeFactionHostile : WorldObjectCompProperties
	{
		// Token: 0x06003F72 RID: 16242 RVA: 0x00158D79 File Offset: 0x00156F79
		public WorldObjectCompProperties_TimedMakeFactionHostile()
		{
			this.compClass = typeof(TimedMakeFactionHostile);
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00158D91 File Offset: 0x00156F91
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedMakeFactionHostile but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
