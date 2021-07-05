using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000228 RID: 552
	public abstract class SkyOverlay
	{
		// Token: 0x17000302 RID: 770
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x0005951E File Offset: 0x0005771E
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

		// Token: 0x06000FAD RID: 4013 RVA: 0x00059554 File Offset: 0x00057754
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00059570 File Offset: 0x00057770
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

		// Token: 0x06000FAF RID: 4015 RVA: 0x00059658 File Offset: 0x00057858
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
				position2.y = AltitudeLayer.Weather.AltitudeFor() + 0.04054054f;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00059722 File Offset: 0x00057922
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

		// Token: 0x04000C56 RID: 3158
		public Material worldOverlayMat;

		// Token: 0x04000C57 RID: 3159
		public Material screenOverlayMat;

		// Token: 0x04000C58 RID: 3160
		protected float worldOverlayPanSpeed1;

		// Token: 0x04000C59 RID: 3161
		protected float worldOverlayPanSpeed2;

		// Token: 0x04000C5A RID: 3162
		protected Vector2 worldPanDir1;

		// Token: 0x04000C5B RID: 3163
		protected Vector2 worldPanDir2;
	}
}
