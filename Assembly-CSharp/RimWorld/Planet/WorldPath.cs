using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002008 RID: 8200
	public class WorldPath : IDisposable
	{
		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x0600ADAB RID: 44459 RVA: 0x00071053 File Offset: 0x0006F253
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x0600ADAC RID: 44460 RVA: 0x00071065 File Offset: 0x0006F265
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x0600ADAD RID: 44461 RVA: 0x0007106D File Offset: 0x0006F26D
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x0600ADAE RID: 44462 RVA: 0x00071077 File Offset: 0x0006F277
		public List<int> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x0600ADAF RID: 44463 RVA: 0x0007107F File Offset: 0x0006F27F
		public int FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x0600ADB0 RID: 44464 RVA: 0x00071099 File Offset: 0x0006F299
		public int LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x0600ADB1 RID: 44465 RVA: 0x000710A7 File Offset: 0x0006F2A7
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x0600ADB2 RID: 44466 RVA: 0x000710AE File Offset: 0x0006F2AE
		public void AddNodeAtStart(int tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x0600ADB3 RID: 44467 RVA: 0x000710BC File Offset: 0x0006F2BC
		public void SetupFound(float totalCost)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on WorldPath.NotFound", false);
				return;
			}
			this.totalCostInt = totalCost;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x0600ADB4 RID: 44468 RVA: 0x000710FC File Offset: 0x0006F2FC
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x0600ADB5 RID: 44469 RVA: 0x00071104 File Offset: 0x0006F304
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x0600ADB6 RID: 44470 RVA: 0x0007112B File Offset: 0x0006F32B
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x0600ADB7 RID: 44471 RVA: 0x0007113D File Offset: 0x0006F33D
		public int ConsumeNextNode()
		{
			int result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x0600ADB8 RID: 44472 RVA: 0x00071154 File Offset: 0x0006F354
		public int Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x0600ADB9 RID: 44473 RVA: 0x00328D00 File Offset: 0x00326F00
		public override string ToString()
		{
			if (!this.Found)
			{
				return "WorldPath(not found)";
			}
			if (!this.inUse)
			{
				return "WorldPath(not in use)";
			}
			return string.Concat(new object[]
			{
				"WorldPath(nodeCount= ",
				this.nodes.Count,
				(this.nodes.Count > 0) ? string.Concat(new object[]
				{
					" first=",
					this.FirstNode,
					" last=",
					this.LastNode
				}) : "",
				" cost=",
				this.totalCostInt,
				" )"
			});
		}

		// Token: 0x0600ADBA RID: 44474 RVA: 0x00328DBC File Offset: 0x00326FBC
		public void DrawPath(Caravan pathingCaravan)
		{
			if (!this.Found)
			{
				return;
			}
			if (this.NodesLeftCount > 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				float d = 0.05f;
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = worldGrid.GetTileCenter(this.Peek(i));
					Vector3 vector = worldGrid.GetTileCenter(this.Peek(i + 1));
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector);
				}
				if (pathingCaravan != null)
				{
					Vector3 a2 = pathingCaravan.DrawPos;
					Vector3 vector2 = worldGrid.GetTileCenter(this.Peek(0));
					a2 += a2.normalized * d;
					vector2 += vector2.normalized * d;
					if ((a2 - vector2).sqrMagnitude > 0.005f)
					{
						GenDraw.DrawWorldLineBetween(a2, vector2);
					}
				}
			}
		}

		// Token: 0x0400774E RID: 30542
		private List<int> nodes = new List<int>(128);

		// Token: 0x0400774F RID: 30543
		private float totalCostInt;

		// Token: 0x04007750 RID: 30544
		private int curNodeIndex;

		// Token: 0x04007751 RID: 30545
		public bool inUse;
	}
}
