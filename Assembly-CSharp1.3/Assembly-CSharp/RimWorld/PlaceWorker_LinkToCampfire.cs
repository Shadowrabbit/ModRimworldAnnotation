using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001566 RID: 5478
	public class PlaceWorker_LinkToCampfire : PlaceWorker
	{
		// Token: 0x060081B9 RID: 33209 RVA: 0x002DDB58 File Offset: 0x002DBD58
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawRadiusRing(center, this.range);
			List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, this.range, ThingDefOf.Campfire, null);
			for (int i = 0; i < forCell.Count; i++)
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[i].TrueCenter(), SimpleColor.Green, 0.2f);
			}
		}

		// Token: 0x040050BA RID: 20666
		public float range = 11.9f;
	}
}
