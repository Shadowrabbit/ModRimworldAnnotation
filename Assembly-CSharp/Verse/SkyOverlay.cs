using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000319 RID: 793
	public abstract class SkyOverlay
	{
		// Token: 0x170003AE RID: 942
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x0001489B File Offset: 0x00012A9B
		public Color OverlayColor
		{
			set
			{
				if (this.worldOverlayMat != null)
				{
					this.worldOverlayMat.color = value;
				}
				if (this.screenOverlayMat != null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x000148D1 File Offset: 0x00012AD1
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000CE034 File Offset: 0x000CC234
		public virtual void TickOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset("_MainTex", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					this.worldOverlayMat.SetTextureOffset("_MainTex2", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale("_MainTex2").x);
				}
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000CE11C File Offset: 0x000CC31C
		public void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if (this.screenOverlayMat != null)
			{
				float num = Find.Camera.orthographicSize * 2f;
				Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
				Vector3 position2 = Find.Camera.transform.position;
				position2.y = AltitudeLayer.Weather.AltitudeFor() + 0.042857144f;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000148EA File Offset: 0x00012AEA
		public override string ToString()
		{
			if (this.worldOverlayMat != null)
			{
				return this.worldOverlayMat.name;
			}
			if (this.screenOverlayMat != null)
			{
				return this.screenOverlayMat.name;
			}
			return "NoOverlayOverlay";
		}

		// Token: 0x04000FE1 RID: 4065
		public Material worldOverlayMat;

		// Token: 0x04000FE2 RID: 4066
		public Material screenOverlayMat;

		// Token: 0x04000FE3 RID: 4067
		protected float worldOverlayPanSpeed1;

		// Token: 0x04000FE4 RID: 4068
		protected float worldOverlayPanSpeed2;

		// Token: 0x04000FE5 RID: 4069
		protected Vector2 worldPanDir1;

		// Token: 0x04000FE6 RID: 4070
		protected Vector2 worldPanDir2;
	}
}
