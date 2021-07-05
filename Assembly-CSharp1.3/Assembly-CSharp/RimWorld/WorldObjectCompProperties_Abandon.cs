using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A37 RID: 2615
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x06003F5C RID: 16220 RVA: 0x00158BF6 File Offset: 0x00156DF6
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00158C0E File Offset: 0x00156E0E
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
