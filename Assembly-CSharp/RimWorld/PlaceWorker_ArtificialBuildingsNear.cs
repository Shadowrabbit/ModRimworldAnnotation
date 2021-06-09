using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEB RID: 7659
	public class PlaceWorker_ArtificialBuildingsNear : PlaceWorker
	{
		// Token: 0x0600A614 RID: 42516 RVA: 0x00302ABC File Offset: 0x00300CBC
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
