using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x02000192 RID: 402
	public class DrawBatch
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x0003DA4C File Offset: 0x0003BC4C
		public DrawBatchPropertyBlock GetPropertyBlock()
		{
			DrawBatchPropertyBlock drawBatchPropertyBlock;
			if (this.propertyBlockCache.Count == 0)
			{
				drawBatchPropertyBlock = new DrawBatchPropertyBlock();
				this.myPropertyBlocks.Add(drawBatchPropertyBlock);
			}
			else
			{
				drawBatchPropertyBlock = this.propertyBlockCache.Pop<DrawBatchPropertyBlock>();
			}
			if (DrawBatch.PropertyBlockLeakDebug)
			{
				drawBatchPropertyBlock.leakDebugString = "Allocated from:\n\n---------------\n\n" + StackTraceUtility.ExtractStackTrace();
			}
			return drawBatchPropertyBlock;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0003DAA6 File Offset: 0x0003BCA6
		public void ReturnPropertyBlock(DrawBatchPropertyBlock propertyBlock)
		{
			if (this.myPropertyBlocks.Contains(propertyBlock))
			{
				this.propertyBlockCache.Add(propertyBlock);
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0003DAC2 File Offset: 0x0003BCC2
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Color? color = null, bool renderInstanced = false, DrawBatchPropertyBlock propertyBlock = null)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, propertyBlock)).Add(matrix, color);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003DADF File Offset: 0x0003BCDF
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, bool renderInstanced = false)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, null)).Add(matrix);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0003DAFC File Offset: 0x0003BCFC
		public void Flush(bool draw = true)
		{
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			this.tmpPropertyBlocks.Clear();
			this.tmpPropertyBlocks.AddRange(this.propertyBlockCache);
			try
			{
				foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in this.batches)
				{
					DrawBatch.BatchKey key = keyValuePair.Key;
					try
					{
						foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
						{
							DrawBatch.BatchData batchData2 = batchData;
							if (draw)
							{
								this.tmpPropertyBlock.Clear();
								if (key.propertyBlock != null)
								{
									key.propertyBlock.Write(this.tmpPropertyBlock);
								}
								if (key.renderInstanced)
								{
									key.material.enableInstancing = true;
									if (batchData2.hasAnyColors)
									{
										this.tmpPropertyBlock.SetVectorArray("_Color", batchData2.colors);
									}
									Graphics.DrawMeshInstanced(key.mesh, 0, key.material, batchData.matrices, batchData.ptr, this.tmpPropertyBlock, ShadowCastingMode.On, true, key.layer);
								}
								else
								{
									for (int i = 0; i < batchData2.ptr; i++)
									{
										Matrix4x4 matrix = batchData2.matrices[i];
										Vector4 v = batchData2.colors[i];
										if (batchData2.hasAnyColors)
										{
											this.tmpPropertyBlock.SetColor("_Color", v);
										}
										Graphics.DrawMesh(key.mesh, matrix, key.material, key.layer, null, 0, this.tmpPropertyBlock);
									}
								}
							}
							batchData2.Clear();
							this.batchDataListCache.Add(batchData2);
						}
					}
					finally
					{
						if (key.propertyBlock != null && this.myPropertyBlocks.Contains(key.propertyBlock))
						{
							this.tmpPropertyBlocks.Add(key.propertyBlock);
							key.propertyBlock.Clear();
							this.propertyBlockCache.Add(key.propertyBlock);
						}
						this.batchListCache.Add(keyValuePair.Value);
						keyValuePair.Value.Clear();
					}
				}
			}
			finally
			{
				foreach (DrawBatchPropertyBlock drawBatchPropertyBlock in this.myPropertyBlocks)
				{
					if (!this.tmpPropertyBlocks.Contains(drawBatchPropertyBlock))
					{
						Log.Warning("Property block from FleckDrawBatch leaked!" + ((drawBatchPropertyBlock.leakDebugString == null) ? null : ("Leak debug information: \n" + drawBatchPropertyBlock.leakDebugString)));
					}
				}
				HashSet<DrawBatchPropertyBlock> hashSet = this.myPropertyBlocks;
				this.myPropertyBlocks = this.tmpPropertyBlocks;
				this.tmpPropertyBlocks = hashSet;
				this.batches.Clear();
				this.lastBatchKey = default(DrawBatch.BatchKey);
				this.lastBatchList = null;
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0003DE58 File Offset: 0x0003C058
		private DrawBatch.BatchData GetBatchDataForInsertion(DrawBatch.BatchKey key)
		{
			List<DrawBatch.BatchData> list;
			if (this.lastBatchList != null && key.GetHashCode() == this.lastBatchKey.GetHashCode() && key.Equals(this.lastBatchKey))
			{
				list = this.lastBatchList;
			}
			else
			{
				if (!this.batches.TryGetValue(key, out list))
				{
					list = ((this.batchListCache.Count == 0) ? new List<DrawBatch.BatchData>() : this.batchListCache.Pop<List<DrawBatch.BatchData>>());
					this.batches.Add(key, list);
					list.Add((this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>());
				}
				this.lastBatchList = list;
				this.lastBatchKey = key;
			}
			int index = list.Count - 1;
			if (list[index].ptr < 1023)
			{
				return list[index];
			}
			DrawBatch.BatchData batchData = (this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>();
			list.Add(batchData);
			return batchData;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003DF64 File Offset: 0x0003C164
		public void MergeWith(DrawBatch other)
		{
			foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in other.batches)
			{
				foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
				{
					while (batchData.ptr > 0)
					{
						DrawBatch.BatchData batchDataForInsertion = this.GetBatchDataForInsertion(keyValuePair.Key);
						int num = Mathf.Min(batchData.ptr, 1023 - batchDataForInsertion.ptr);
						Array.Copy(batchData.matrices, 0, batchDataForInsertion.matrices, batchDataForInsertion.ptr, num);
						Array.Copy(batchData.colors, 0, batchDataForInsertion.colors, batchDataForInsertion.ptr, num);
						batchDataForInsertion.ptr += num;
						batchData.ptr -= num;
					}
				}
			}
		}

		// Token: 0x0400095F RID: 2399
		private Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>> batches = new Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>>();

		// Token: 0x04000960 RID: 2400
		private List<DrawBatch.BatchData> batchDataListCache = new List<DrawBatch.BatchData>();

		// Token: 0x04000961 RID: 2401
		private List<List<DrawBatch.BatchData>> batchListCache = new List<List<DrawBatch.BatchData>>();

		// Token: 0x04000962 RID: 2402
		private HashSet<DrawBatchPropertyBlock> myPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000963 RID: 2403
		private List<DrawBatchPropertyBlock> propertyBlockCache = new List<DrawBatchPropertyBlock>();

		// Token: 0x04000964 RID: 2404
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04000965 RID: 2405
		private HashSet<DrawBatchPropertyBlock> tmpPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000966 RID: 2406
		private const int MaxCountPerBatch = 1023;

		// Token: 0x04000967 RID: 2407
		private static bool PropertyBlockLeakDebug;

		// Token: 0x04000968 RID: 2408
		private object lockObj = new object();

		// Token: 0x04000969 RID: 2409
		private DrawBatch.BatchKey lastBatchKey;

		// Token: 0x0400096A RID: 2410
		private List<DrawBatch.BatchData> lastBatchList;

		// Token: 0x0200195B RID: 6491
		private class BatchData
		{
			// Token: 0x0600981E RID: 38942 RVA: 0x0035E16B File Offset: 0x0035C36B
			public BatchData()
			{
				this.matrices = new Matrix4x4[1023];
				this.colors = new Vector4[1023];
				this.ptr = 0;
			}

			// Token: 0x0600981F RID: 38943 RVA: 0x0035E19A File Offset: 0x0035C39A
			public void Clear()
			{
				this.ptr = 0;
				this.hasAnyColors = false;
			}

			// Token: 0x06009820 RID: 38944 RVA: 0x0035E1AA File Offset: 0x0035C3AA
			public void Add(Matrix4x4 matrix)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = DrawBatch.BatchData.WhiteColor;
				this.ptr++;
			}

			// Token: 0x06009821 RID: 38945 RVA: 0x0035E1E4 File Offset: 0x0035C3E4
			public void Add(Matrix4x4 matrix, Color? color)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = (color ?? DrawBatch.BatchData.WhiteColor);
				this.ptr++;
				this.hasAnyColors = true;
			}

			// Token: 0x0400614C RID: 24908
			public Matrix4x4[] matrices;

			// Token: 0x0400614D RID: 24909
			public int ptr;

			// Token: 0x0400614E RID: 24910
			public Vector4[] colors;

			// Token: 0x0400614F RID: 24911
			public bool hasAnyColors;

			// Token: 0x04006150 RID: 24912
			private static readonly Vector4 WhiteColor = Color.white;
		}

		// Token: 0x0200195C RID: 6492
		private struct BatchKey : IEquatable<DrawBatch.BatchKey>
		{
			// Token: 0x06009823 RID: 38947 RVA: 0x0035E260 File Offset: 0x0035C460
			public BatchKey(Mesh mesh, Material material, int layer, bool renderInstanced, DrawBatchPropertyBlock propertyBlock)
			{
				this.mesh = mesh;
				this.material = material;
				this.layer = layer;
				this.renderInstanced = renderInstanced;
				this.propertyBlock = propertyBlock;
				this.hash = mesh.GetHashCode();
				this.hash = Gen.HashCombineInt(this.hash, material.GetHashCode());
				this.hash = Gen.HashCombineInt(this.hash, layer | (renderInstanced ? 1 : 0) << 8);
				this.hash = ((propertyBlock == null) ? this.hash : Gen.HashCombineInt(this.hash, propertyBlock.GetHashCode()));
			}

			// Token: 0x06009824 RID: 38948 RVA: 0x0035E2F8 File Offset: 0x0035C4F8
			public override bool Equals(object obj)
			{
				if (obj != null && obj is DrawBatch.BatchKey)
				{
					DrawBatch.BatchKey other = (DrawBatch.BatchKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06009825 RID: 38949 RVA: 0x0035E324 File Offset: 0x0035C524
			public bool Equals(DrawBatch.BatchKey other)
			{
				return this.mesh == other.mesh && this.material == other.material && this.layer == other.layer && this.renderInstanced == other.renderInstanced && this.propertyBlock == other.propertyBlock;
			}

			// Token: 0x06009826 RID: 38950 RVA: 0x0035E379 File Offset: 0x0035C579
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x04006151 RID: 24913
			public readonly Mesh mesh;

			// Token: 0x04006152 RID: 24914
			public readonly Material material;

			// Token: 0x04006153 RID: 24915
			public readonly int layer;

			// Token: 0x04006154 RID: 24916
			public readonly bool renderInstanced;

			// Token: 0x04006155 RID: 24917
			public readonly DrawBatchPropertyBlock propertyBlock;

			// Token: 0x04006156 RID: 24918
			private int hash;
		}
	}
}
