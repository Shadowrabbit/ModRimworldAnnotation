using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEF RID: 7663
	public static class ApparelGraphicRecordGetter
	{
		// Token: 0x0600A61C RID: 42524 RVA: 0x00302C24 File Offset: 0x00300E24
		public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, out ApparelGraphicRecord rec)
		{
			if (bodyType == null)
			{
				Log.Error("Getting apparel graphic with undefined body type.", false);
				bodyType = BodyTypeDefOf.Male;
			}
			if (apparel.def.apparel.wornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				return false;
			}
			string path;
			if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || PawnRenderer.RenderAsPack(apparel) || apparel.def.apparel.wornGraphicPath == BaseContent.PlaceholderImagePath)
			{
				path = apparel.def.apparel.wornGraphicPath;
			}
			else
			{
				path = apparel.def.apparel.wornGraphicPath + "_" + bodyType.defName;
			}
			Shader shader = ShaderDatabase.Cutout;
			if (apparel.def.apparel.useWornGraphicMask)
			{
				shader = ShaderDatabase.CutoutComplex;
			}
			Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader, apparel.def.graphicData.drawSize, apparel.DrawColor);
			rec = new ApparelGraphicRecord(graphic, apparel);
			return true;
		}
	}
}
