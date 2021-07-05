using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035C RID: 860
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001866 RID: 6246 RVA: 0x00090CAB File Offset: 0x0008EEAB
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00090CB8 File Offset: 0x0008EEB8
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
			this.drawSize = subGraphic.drawSize;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00090CDC File Offset: 0x0008EEDC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Material matSingle = this.subGraphic.MatSingle;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0, null, 0);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00090D44 File Offset: 0x0008EF44
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00090D60 File Offset: 0x0008EF60
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data,
				drawSize = this.drawSize
			};
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00090D94 File Offset: 0x0008EF94
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			float num = 0f;
			if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			this.subGraphic.Print(layer, thing, num);
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00090DDF File Offset: 0x0008EFDF
		public override void TryInsertIntoAtlas(TextureAtlasGroup group)
		{
			this.subGraphic.TryInsertIntoAtlas(group);
		}

		// Token: 0x0400109D RID: 4253
		private Graphic subGraphic;

		// Token: 0x0400109E RID: 4254
		private float maxAngle;
	}
}
