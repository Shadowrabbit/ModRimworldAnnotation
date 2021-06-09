using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E4 RID: 1252
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06001F3F RID: 7999 RVA: 0x0001B938 File Offset: 0x00019B38
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000FF5D0 File Offset: 0x000FD7D0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000FF668 File Offset: 0x000FD868
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0001B961 File Offset: 0x00019B61
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x040015FB RID: 5627
		private Mesh shadowMesh;

		// Token: 0x040015FC RID: 5628
		private ShadowData shadowInfo;

		// Token: 0x040015FD RID: 5629
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x040015FE RID: 5630
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
