using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE6 RID: 7654
	public class PlaceWorker_TurretTop : PlaceWorker
	{
		// Token: 0x0600A60A RID: 42506 RVA: 0x0030286C File Offset: 0x00300A6C
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Single>(def.building.turretGunDef.graphicData.texPath, ShaderDatabase.Cutout, new Vector2(def.building.turretTopDrawSize, def.building.turretTopDrawSize), Color.white), def, ghostCol).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, (float)TurretTop.ArtworkRotation);
		}
	}
}
