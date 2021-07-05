using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035D RID: 861
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x0600186D RID: 6253 RVA: 0x00090DED File Offset: 0x0008EFED
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00090E18 File Offset: 0x0008F018
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00090EB0 File Offset: 0x0008F0B0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00090F19 File Offset: 0x0008F119
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x0400109F RID: 4255
		private Mesh shadowMesh;

		// Token: 0x040010A0 RID: 4256
		private ShadowData shadowInfo;

		// Token: 0x040010A1 RID: 4257
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x040010A2 RID: 4258
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
