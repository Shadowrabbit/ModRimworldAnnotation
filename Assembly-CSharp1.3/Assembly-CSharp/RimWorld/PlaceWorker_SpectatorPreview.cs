using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001561 RID: 5473
	[StaticConstructorOnStartup]
	public class PlaceWorker_SpectatorPreview : PlaceWorker
	{
		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x060081A7 RID: 33191 RVA: 0x002DD55F File Offset: 0x002DB75F
		private static Graphic SpectatingPawnsGraphic
		{
			get
			{
				if (PlaceWorker_SpectatorPreview.spectatingPawnsGraphic == null)
				{
					PlaceWorker_SpectatorPreview.spectatingPawnsGraphic = GraphicDatabase.Get<Graphic_Multi>("UI/Overlays/PawnsSpectating", ShaderDatabase.Transparent);
				}
				return PlaceWorker_SpectatorPreview.spectatingPawnsGraphic;
			}
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x002DD584 File Offset: 0x002DB784
		public static void DrawSpectatorPreview(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, bool useArrow, out CellRect rect, Thing thing = null)
		{
			rect = GenAdj.OccupiedRect(center, rot, def.size);
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			RitualFocusProperties ritualFocus = def.ritualFocus;
			if (ritualFocus == null)
			{
				ThingDef thingDef = def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ritualFocus = thingDef.ritualFocus;
				}
			}
			if (ritualFocus == null)
			{
				return;
			}
			foreach (SpectateRectSide spectateRectSide in ritualFocus.allowedSpectateSides.GetAllSelectedItems<SpectateRectSide>())
			{
				if (spectateRectSide.ValidSingleSide())
				{
					Rot4 rot2 = spectateRectSide.Rotated(rot).AsRot4();
					if (useArrow)
					{
						GenDraw.DrawArrowRotated(spectateRectSide.GraphicOffsetForRect(center, rect, rot, Vector2.zero), rot2.AsAngle, true);
					}
					Vector2 vector = rot2.IsHorizontal ? new Vector2(2f, 4f) : new Vector2(4f, 2f);
					bool flag = (spectateRectSide & SpectateRectSide.Horizontal) != SpectateRectSide.Horizontal;
					Vector2 vector2 = vector - new Vector2(0.5f, 0.5f);
					GenDraw.DrawMeshNowOrLater(PlaceWorker_SpectatorPreview.SpectatingPawnsGraphic.MeshAt(rot2.Opposite), Matrix4x4.TRS(spectateRectSide.GraphicOffsetForRect(center, rect, rot, flag ? vector2 : (-vector2)) + new Vector3(0f, 8f, 0f), Quaternion.identity, new Vector3(vector.x, 1f, vector.y)), PlaceWorker_SpectatorPreview.SpectatingPawnsGraphic.MatAt(rot2.Opposite, null), false);
				}
			}
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x002DD738 File Offset: 0x002DB938
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CellRect cellRect;
			PlaceWorker_SpectatorPreview.DrawSpectatorPreview(def, center, rot, ghostCol, true, out cellRect, thing);
		}

		// Token: 0x040050B4 RID: 20660
		private static Graphic spectatingPawnsGraphic;
	}
}
