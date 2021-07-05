using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001569 RID: 5481
	public static class ApparelGraphicRecordGetter
	{
		// Token: 0x060081BE RID: 33214 RVA: 0x002DDC28 File Offset: 0x002DBE28
		public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, out ApparelGraphicRecord rec)
		{
			if (bodyType == null)
			{
				Log.Error("Getting apparel graphic with undefined body type.");
				bodyType = BodyTypeDefOf.Male;
			}
			if (apparel.WornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				return false;
			}
			string path;
			if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || PawnRenderer.RenderAsPack(apparel) || apparel.WornGraphicPath == BaseContent.PlaceholderImagePath || apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath)
			{
				path = apparel.WornGraphicPath;
			}
			else
			{
				path = apparel.WornGraphicPath + "_" + bodyType.defName;
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
