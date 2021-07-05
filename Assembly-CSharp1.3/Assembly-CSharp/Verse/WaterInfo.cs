using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C7 RID: 455
	public class WaterInfo : MapComponent
	{
		// Token: 0x06000D37 RID: 3383 RVA: 0x00046C4A File Offset: 0x00044E4A
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00046C5E File Offset: 0x00044E5E
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				UnityEngine.Object.Destroy(this.riverOffsetTexture);
			});
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00046C74 File Offset: 0x00044E74
		public void SetTextures()
		{
			Camera subcamera = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.WaterDepth);
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOutputTex, subcamera.targetTexture);
			if (this.riverOffsetTexture == null && this.riverOffsetMap != null && this.riverOffsetMap.Length != 0)
			{
				this.riverOffsetTexture = new Texture2D(this.map.Size.x + 4, this.map.Size.z + 4, TextureFormat.RGFloat, false);
				this.riverOffsetTexture.LoadRawTextureData(this.riverOffsetMap);
				this.riverOffsetTexture.wrapMode = TextureWrapMode.Clamp;
				this.riverOffsetTexture.Apply();
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOffsetTex, this.riverOffsetTexture);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00046D2C File Offset: 0x00044F2C
		public Vector3 GetWaterMovement(Vector3 position)
		{
			if (this.riverOffsetMap == null)
			{
				return Vector3.zero;
			}
			if (this.riverFlowMap == null)
			{
				this.GenerateRiverFlowMap();
			}
			IntVec3 intVec = new IntVec3(Mathf.FloorToInt(position.x), 0, Mathf.FloorToInt(position.z));
			IntVec3 c = new IntVec3(Mathf.FloorToInt(position.x) + 1, 0, Mathf.FloorToInt(position.z) + 1);
			if (!this.riverFlowMapBounds.Contains(intVec) || !this.riverFlowMapBounds.Contains(c))
			{
				return Vector3.zero;
			}
			int num = this.riverFlowMapBounds.IndexOf(intVec);
			int num2 = num + 1;
			int num3 = num + this.riverFlowMapBounds.Width;
			int num4 = num3 + 1;
			Vector3 a = Vector3.Lerp(new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]), new Vector3(this.riverFlowMap[num2 * 2], 0f, this.riverFlowMap[num2 * 2 + 1]), position.x - Mathf.Floor(position.x));
			Vector3 b = Vector3.Lerp(new Vector3(this.riverFlowMap[num3 * 2], 0f, this.riverFlowMap[num3 * 2 + 1]), new Vector3(this.riverFlowMap[num4 * 2], 0f, this.riverFlowMap[num4 * 2 + 1]), position.x - Mathf.Floor(position.x));
			return Vector3.Lerp(a, b, position.z - (float)Mathf.FloorToInt(position.z));
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00046EAC File Offset: 0x000450AC
		public void GenerateRiverFlowMap()
		{
			if (this.riverOffsetMap == null)
			{
				return;
			}
			this.riverFlowMapBounds = new CellRect(-2, -2, this.map.Size.x + 4, this.map.Size.z + 4);
			this.riverFlowMap = new float[this.riverFlowMapBounds.Area * 2];
			float[] array = new float[this.riverFlowMapBounds.Area * 2];
			Buffer.BlockCopy(this.riverOffsetMap, 0, array, 0, array.Length * 4);
			for (int i = this.riverFlowMapBounds.minZ; i <= this.riverFlowMapBounds.maxZ; i++)
			{
				int newZ = (i == this.riverFlowMapBounds.minZ) ? i : (i - 1);
				int newZ2 = (i == this.riverFlowMapBounds.maxZ) ? i : (i + 1);
				float num = (float)((i == this.riverFlowMapBounds.minZ || i == this.riverFlowMapBounds.maxZ) ? 1 : 2);
				for (int j = this.riverFlowMapBounds.minX; j <= this.riverFlowMapBounds.maxX; j++)
				{
					int newX = (j == this.riverFlowMapBounds.minX) ? j : (j - 1);
					int newX2 = (j == this.riverFlowMapBounds.maxX) ? j : (j + 1);
					float num2 = (float)((j == this.riverFlowMapBounds.minX || j == this.riverFlowMapBounds.maxX) ? 1 : 2);
					float x = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX2, 0, i)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX, 0, i)) * 2 + 1]) / num2;
					float z = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ2)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ)) * 2 + 1]) / num;
					Vector3 vector = new Vector3(x, 0f, z);
					if (vector.magnitude > 0.0001f)
					{
						vector = vector.normalized / vector.magnitude;
						int num3 = this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, i)) * 2;
						this.riverFlowMap[num3] = vector.x;
						this.riverFlowMap[num3 + 1] = vector.z;
					}
				}
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00047108 File Offset: 0x00045308
		public override void ExposeData()
		{
			base.ExposeData();
			DataExposeUtility.ByteArray(ref this.riverOffsetMap, "riverOffsetMap");
			this.GenerateRiverFlowMap();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00047128 File Offset: 0x00045328
		public void DebugDrawRiver()
		{
			for (int i = 0; i < this.riverDebugData.Count; i += 2)
			{
				GenDraw.DrawLineBetween(this.riverDebugData[i], this.riverDebugData[i + 1], SimpleColor.Magenta, 0.2f);
			}
		}

		// Token: 0x04000AD9 RID: 2777
		public byte[] riverOffsetMap;

		// Token: 0x04000ADA RID: 2778
		public Texture2D riverOffsetTexture;

		// Token: 0x04000ADB RID: 2779
		public List<Vector3> riverDebugData = new List<Vector3>();

		// Token: 0x04000ADC RID: 2780
		public float[] riverFlowMap;

		// Token: 0x04000ADD RID: 2781
		public CellRect riverFlowMapBounds;

		// Token: 0x04000ADE RID: 2782
		public const int RiverOffsetMapBorder = 2;
	}
}
