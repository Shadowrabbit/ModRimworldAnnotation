using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000358 RID: 856
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x000903DA File Offset: 0x0008E5DA
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x000903E8 File Offset: 0x0008E5E8
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Color color = base.Color * mote.instanceColor;
			color.a *= alpha;
			Vector3 exactScale = mote.exactScale;
			exactScale.x *= this.data.drawSize.x;
			exactScale.z *= this.data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
			Material matSingle = this.MatSingle;
			if (!this.ForcePropertyBlock && color.IndistinguishableFrom(matSingle.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0, Graphic_Mote.propertyBlock);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x000904E8 File Offset: 0x0008E6E8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Mote(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x04001098 RID: 4248
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
