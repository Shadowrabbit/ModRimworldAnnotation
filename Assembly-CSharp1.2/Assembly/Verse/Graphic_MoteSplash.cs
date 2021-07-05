using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E0 RID: 1248
	public class Graphic_MoteSplash : Graphic_Mote
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000FF0AC File Offset: 0x000FD2AC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteSplash moteSplash = (MoteSplash)thing;
			float alpha = moteSplash.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, alpha));
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, moteSplash.CalculatedShockwaveSpan());
			base.DrawMoteInternal(loc, rot, thingDef, thing, SubcameraDefOf.WaterDepth.LayerId);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000FF120 File Offset: 0x000FD320
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"MoteSplash(path=",
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
