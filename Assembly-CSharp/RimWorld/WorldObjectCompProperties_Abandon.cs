using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F54 RID: 3924
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x0600564D RID: 22093 RVA: 0x0003BE14 File Offset: 0x0003A014
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x0003BE2C File Offset: 0x0003A02C
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_Abandon but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
