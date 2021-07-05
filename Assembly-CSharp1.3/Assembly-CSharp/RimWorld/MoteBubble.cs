using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B4 RID: 4276
	[StaticConstructorOnStartup]
	public class MoteBubble : MoteDualAttached
	{
		// Token: 0x06006623 RID: 26147 RVA: 0x00227C00 File Offset: 0x00225E00
		public void SetupMoteBubble(Texture2D icon, Pawn target, Color? iconColor = null)
		{
			this.iconMat = MaterialPool.MatFrom(icon, ShaderDatabase.TransparentPostLight, Color.white);
			this.iconMatPropertyBlock = new MaterialPropertyBlock();
			this.arrowTarget = target;
			if (iconColor != null)
			{
				this.iconMatPropertyBlock.SetColor("_Color", iconColor.Value);
			}
		}

		// Token: 0x06006624 RID: 26148 RVA: 0x00227C58 File Offset: 0x00225E58
		public override void Draw()
		{
			base.Draw();
			if (this.iconMat != null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.01f;
				float alpha = this.Alpha;
				if (alpha <= 0f)
				{
					return;
				}
				Color instanceColor = this.instanceColor;
				instanceColor.a *= alpha;
				Material material = this.iconMat;
				if (instanceColor != material.color)
				{
					material = MaterialPool.MatFrom((Texture2D)material.mainTexture, material.shader, instanceColor);
				}
				Vector3 s = new Vector3(this.def.graphicData.drawSize.x * 0.64f, 1f, this.def.graphicData.drawSize.y * 0.64f);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(drawPos, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0, null, 0, this.iconMatPropertyBlock);
			}
			if (this.arrowTarget != null)
			{
				Quaternion rotation = Quaternion.AngleAxis((this.arrowTarget.TrueCenter() - this.DrawPos).AngleFlat(), Vector3.up);
				Vector3 vector = this.DrawPos;
				vector.y -= 0.01f;
				vector += 0.6f * (rotation * Vector3.forward);
				Graphics.DrawMesh(MeshPool.plane05, vector, rotation, MoteBubble.InteractionArrowTex, 0);
			}
		}

		// Token: 0x040039AF RID: 14767
		public Material iconMat;

		// Token: 0x040039B0 RID: 14768
		public Pawn arrowTarget;

		// Token: 0x040039B1 RID: 14769
		public MaterialPropertyBlock iconMatPropertyBlock;

		// Token: 0x040039B2 RID: 14770
		private static readonly Material InteractionArrowTex = MaterialPool.MatFrom("Things/Mote/InteractionArrow");
	}
}
