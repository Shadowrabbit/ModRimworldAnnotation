using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A3B RID: 2619
	public class WorldObjectCompProperties_EscapeShip : WorldObjectCompProperties
	{
		// Token: 0x06003F64 RID: 16228 RVA: 0x00158C8D File Offset: 0x00156E8D
		public WorldObjectCompProperties_EscapeShip()
		{
			this.compClass = typeof(EscapeShipComp);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00158CA5 File Offset: 0x00156EA5
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_EscapeShip but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
