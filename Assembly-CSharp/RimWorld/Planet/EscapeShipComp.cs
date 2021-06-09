using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002197 RID: 8599
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x0600B7A2 RID: 47010 RVA: 0x0034F090 File Offset: 0x0034D290
		public override void PostMapGenerate()
		{
			Building building = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_Reactor).FirstOrDefault<Building>();
			Building_ShipReactor building_ShipReactor;
			if (building != null && (building_ShipReactor = (building as Building_ShipReactor)) != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		// Token: 0x0600B7A3 RID: 47011 RVA: 0x00077172 File Offset: 0x00075372
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in CaravanArrivalAction_VisitEscapeShip.GetFloatMenuOptions(caravan, (MapParent)this.parent))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}
	}
}
