using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017ED RID: 6125
	public class AbandonComp : WorldObjectComp
	{
		// Token: 0x06008EDB RID: 36571 RVA: 0x00333FEA File Offset: 0x003321EA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = this.parent as MapParent;
			if (mapParent.HasMap && mapParent.Faction == Faction.OfPlayer)
			{
				yield return SettlementAbandonUtility.AbandonCommand(mapParent);
			}
			yield break;
		}
	}
}
