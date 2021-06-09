using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEC RID: 7660
	public class PlaceWorker_MeditationOffsetBuildingsNear : PlaceWorker
	{
		// Token: 0x0600A616 RID: 42518 RVA: 0x00302AFC File Offset: 0x00300CFC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return;
			}
			FocusStrengthOffset_BuildingDefs focusStrengthOffset_BuildingDefs = ((CompProperties_MeditationFocus)((def.IsFrame || def.IsBlueprint) ? ((ThingDef)def.entityDefToBuild).CompDefFor<CompMeditationFocus>() : def.CompDefFor<CompMeditationFocus>())).offsets.OfType<FocusStrengthOffset_BuildingDefs>().FirstOrDefault<FocusStrengthOffset_BuildingDefs>();
			if (focusStrengthOffset_BuildingDefs != null)
			{
				GenDraw.DrawRadiusRing(center, focusStrengthOffset_BuildingDefs.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
				List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, focusStrengthOffset_BuildingDefs.radius, focusStrengthOffset_BuildingDefs.defs, null);
				int num = 0;
				while (num < forCell.Count && num < focusStrengthOffset_BuildingDefs.maxBuildings)
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[num].TrueCenter(), SimpleColor.Green);
					num++;
				}
			}
		}

		// Token: 0x04007099 RID: 28825
		public static readonly Color RingColor = new Color(0.5f, 0.8f, 0.5f);
	}
}
