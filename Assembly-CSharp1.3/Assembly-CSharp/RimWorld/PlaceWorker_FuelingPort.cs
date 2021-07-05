using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001554 RID: 5460
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x06008183 RID: 33155 RVA: 0x002DCC64 File Offset: 0x002DAE64
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.building == null || !def.building.hasFuelingPort)
			{
				return;
			}
			if (!FuelingPortUtility.GetFuelingPortCell(center, rot).Standable(currentMap))
			{
				return;
			}
			PlaceWorker_FuelingPort.DrawFuelingPortCell(center, rot);
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x002DCCA4 File Offset: 0x002DAEA4
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}

		// Token: 0x040050AE RID: 20654
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);
	}
}
