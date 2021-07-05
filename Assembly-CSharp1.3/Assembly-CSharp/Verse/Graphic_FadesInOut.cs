using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034E RID: 846
	public class Graphic_FadesInOut : Graphic_WithPropertyBlock
	{
		// Token: 0x0600181A RID: 6170 RVA: 0x0008F634 File Offset: 0x0008D834
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			CompFadesInOut compFadesInOut = thing.TryGetComp<CompFadesInOut>();
			if (compFadesInOut == null)
			{
				Log.ErrorOnce(thingDef.defName + ": Graphic_FadesInOut requires CompFadesInOut.", 5643893);
				return;
			}
			this.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(this.color.r, this.color.g, this.color.b, this.color.a * compFadesInOut.Opacity()));
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
