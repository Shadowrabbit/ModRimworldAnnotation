using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F3 RID: 6131
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x06008F03 RID: 36611 RVA: 0x0033482C File Offset: 0x00332A2C
		public override void PostMapGenerate()
		{
			Building building = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_Reactor).FirstOrDefault<Building>();
			Building_ShipReactor building_ShipReactor;
			if (building != null && (building_ShipReactor = (building as Building_ShipReactor)) != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		// Token: 0x06008F04 RID: 36612 RVA: 0x00334872 File Offset: 0x00332A72
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
