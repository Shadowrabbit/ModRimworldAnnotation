using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155D RID: 5469
	public class PlaceWorker_ArtificialBuildingsNear : PlaceWorker
	{
		// Token: 0x0600819C RID: 33180 RVA: 0x002DD350 File Offset: 0x002DB550
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = ((CompProperties_MeditationFocus)def.CompDefFor<CompMeditationFocus>()).offsets.OfType<FocusStrengthOffset_ArtificialBuildings>().FirstOrDefault<FocusStrengthOffset_ArtificialBuildings>();
			if (focusStrengthOffset_ArtificialBuildings != null)
			{
				MeditationUtility.DrawArtificialBuildingOverlay(center, def, Find.CurrentMap, focusStrengthOffset_ArtificialBuildings.radius);
			}
		}
	}
}
