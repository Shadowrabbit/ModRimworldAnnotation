using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000287 RID: 647
	public class WaterInfo : MapComponent
	{
		// Token: 0x060010E3 RID: 4323 RVA: 0x000126FC File Offset: 0x000108FC
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00012710 File Offset: 0x00010910
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				UnityEngine.Object.Destroy(this.riverOffsetTexture);
			});
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000BBFB8 File Offset: 0x000BA1B8
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

		// Token: 0x060010E6 RID: 4326 RVA: 0x000BC070 File Offset: 0x000BA270
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

		// Token: 0x060010E7 RID: 4327 RVA: 0x000BC1F0 File Offset: 0x000BA3F0
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

		// Token: 0x060010E8 RID: 4328 RVA: 0x00012723 File Offset: 0x00010923
		public override void ExposeData()
		{
			base.ExposeData();
			DataExposeUtility.ByteArray(ref this.riverOffsetMap, "riverOffsetMap");
			this.GenerateRiverFlowMap();
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000BC44C File Offset: 0x000BA64C
		public void DebugDrawRiver()
		{
			for (int i = 0; i < this.riverDebugData.Count; i += 2)
			{
				GenDraw.DrawLineBetween(this.riverDebugData[i], this.riverDebugData[i + 1], SimpleColor.Magenta);
			}
		}

		// Token: 0x04000DB5 RID: 3509
		public byte[] riverOffsetMap;

		// Token: 0x04000DB6 RID: 3510
		public Texture2D riverOffsetTexture;

		// Token: 0x04000DB7 RID: 3511
		public List<Vector3> riverDebugData = new List<Vector3>();

		// Token: 0x04000DB8 RID: 3512
		public float[] riverFlowMap;

		// Token: 0x04000DB9 RID: 3513
		public CellRect riverFlowMapBounds;

		// Token: 0x04000DBA RID: 3514
		public const int RiverOffsetMapBorder = 2;
	}
}
