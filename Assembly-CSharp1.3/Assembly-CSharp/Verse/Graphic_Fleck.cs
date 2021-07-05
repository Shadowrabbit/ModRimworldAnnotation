using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	[StaticConstructorOnStartup]
	public class Graphic_Fleck : Graphic_Single
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool AllowInstancing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0002974C File Offset: 0x0002794C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0008F6C4 File Offset: 0x0008D8C4
		public virtual void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			Color value;
			if (drawData.overrideColor != null)
			{
				value = drawData.overrideColor.Value;
			}
			else
			{
				float alpha = drawData.alpha;
				if (alpha <= 0f)
				{
					if (drawData.propertyBlock != null)
					{
						batch.ReturnPropertyBlock(drawData.propertyBlock);
					}
					return;
				}
				value = base.Color * drawData.color;
				value.a *= alpha;
			}
			Vector3 scale = drawData.scale;
			scale.x *= this.data.drawSize.x;
			scale.z *= this.data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawData.pos, Quaternion.AngleAxis(drawData.rotation, Vector3.up), scale);
			Material matSingle = this.MatSingle;
			batch.DrawMesh(MeshPool.plane10, matrix, matSingle, drawData.drawLayer, new Color?(value), this.data.renderInstanced && this.AllowInstancing, drawData.propertyBlock);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0008F7D4 File Offset: 0x0008D9D4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Fleck(path=",
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
