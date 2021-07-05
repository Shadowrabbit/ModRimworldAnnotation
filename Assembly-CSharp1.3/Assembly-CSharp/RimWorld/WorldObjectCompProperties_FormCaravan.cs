using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A3C RID: 2620
	public class WorldObjectCompProperties_FormCaravan : WorldObjectCompProperties
	{
		// Token: 0x06003F67 RID: 16231 RVA: 0x00158CBC File Offset: 0x00156EBC
		public WorldObjectCompProperties_FormCaravan()
		{
			this.compClass = typeof(FormCaravanComp);
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x00158CD4 File Offset: 0x00156ED4
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_FormCaravan but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
