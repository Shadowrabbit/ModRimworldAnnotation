using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155C RID: 5468
	public class PlaceWorker_SmokeCloudMaker : PlaceWorker
	{
		// Token: 0x0600819A RID: 33178 RVA: 0x002DD335 File Offset: 0x002DB535
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawCircleOutline(center.ToVector3Shifted(), def.GetCompProperties<CompProperties_SmokeCloudMaker>().cloudRadius);
		}
	}
}
