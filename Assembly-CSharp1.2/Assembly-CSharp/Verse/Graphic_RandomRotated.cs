using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E3 RID: 1251
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x0001B8BA File Offset: 0x00019ABA
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0001B8C7 File Offset: 0x00019AC7
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
			this.drawSize = subGraphic.drawSize;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000FF568 File Offset: 0x000FD768
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

		// Token: 0x06001F3D RID: 7997 RVA: 0x0001B8E9 File Offset: 0x00019AE9
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0001B905 File Offset: 0x00019B05
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data,
				drawSize = this.drawSize
			};
		}

		// Token: 0x040015F9 RID: 5625
		private Graphic subGraphic;

		// Token: 0x040015FA RID: 5626
		private float maxAngle;
	}
}
