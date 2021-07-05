using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001560 RID: 5472
	public class PlaceWorker_AnimalPenMarker : PlaceWorker
	{
		// Token: 0x060081A3 RID: 33187 RVA: 0x002DD4D8 File Offset: 0x002DB6D8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacingBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			if (thing is Blueprint || thing is Frame)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacedBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			PlaceWorker_AnimalPenMarker.tmpPainter.Paint(center, Find.CurrentMap);
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x002DD52D File Offset: 0x002DB72D
		public override void DrawMouseAttachments(BuildableDef def)
		{
			Find.CurrentMap.animalPenManager.DrawPlacingMouseAttachments(def);
		}

		// Token: 0x040050B1 RID: 20657
		private static readonly AnimalPenGUI.PenPainter tmpPainter = new AnimalPenGUI.PenPainter();

		// Token: 0x040050B2 RID: 20658
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacingBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();

		// Token: 0x040050B3 RID: 20659
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacedBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();
	}
}
