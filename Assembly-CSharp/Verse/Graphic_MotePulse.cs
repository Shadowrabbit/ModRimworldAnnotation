using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DF RID: 1247
	public class Graphic_MotePulse : Graphic_Mote
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000FF008 File Offset: 0x000FD208
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000FF054 File Offset: 0x000FD254
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_MotePulse(path=",
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
