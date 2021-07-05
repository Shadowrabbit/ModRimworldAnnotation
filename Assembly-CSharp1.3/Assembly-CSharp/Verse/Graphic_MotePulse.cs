using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000359 RID: 857
	public class Graphic_MotePulse : Graphic_Mote
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0009054C File Offset: 0x0008E74C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote.AgeSecsPausable);
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000905AC File Offset: 0x0008E7AC
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
