using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000352 RID: 850
	public class Graphic_FleckSplash : Graphic_Fleck
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x0008F8CC File Offset: 0x0008DACC
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, drawData.alpha));
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, drawData.calculatedShockwaveSpan);
			drawData.drawLayer = SubcameraDefOf.WaterDepth.LayerId;
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0008F94C File Offset: 0x0008DB4C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"FleckSplash(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
