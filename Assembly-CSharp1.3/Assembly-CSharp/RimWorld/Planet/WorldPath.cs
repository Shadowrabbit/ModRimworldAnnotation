using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200172C RID: 5932
	public class WorldPath : IDisposable
	{
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x060088C1 RID: 35009 RVA: 0x00312581 File Offset: 0x00310781
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x060088C2 RID: 35010 RVA: 0x00312593 File Offset: 0x00310793
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x060088C3 RID: 35011 RVA: 0x0031259B File Offset: 0x0031079B
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x060088C4 RID: 35012 RVA: 0x003125A5 File Offset: 0x003107A5
		public List<int> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x060088C5 RID: 35013 RVA: 0x003125AD File Offset: 0x003107AD
		public int FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x060088C6 RID: 35014 RVA: 0x003125C7 File Offset: 0x003107C7
		public int LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x060088C7 RID: 35015 RVA: 0x003125D5 File Offset: 0x003107D5
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x060088C8 RID: 35016 RVA: 0x003125DC File Offset: 0x003107DC
		public void AddNodeAtStart(int tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x060088C9 RID: 35017 RVA: 0x003125EA File Offset: 0x003107EA
		public void SetupFound(float totalCost)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on WorldPath.NotFound");
				return;
			}
			this.totalCostInt = totalCost;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x060088CA RID: 35018 RVA: 0x00312629 File Offset: 0x00310829
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x060088CB RID: 35019 RVA: 0x00312631 File Offset: 0x00310831
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x060088CC RID: 35020 RVA: 0x00312658 File Offset: 0x00310858
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x060088CD RID: 35021 RVA: 0x0031266A File Offset: 0x0031086A
		public int ConsumeNextNode()
		{
			int result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x060088CE RID: 35022 RVA: 0x00312681 File Offset: 0x00310881
		public int Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x060088CF RID: 35023 RVA: 0x00312698 File Offset: 0x00310898
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

		// Token: 0x060088D0 RID: 35024 RVA: 0x00312754 File Offset: 0x00310954
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

		// Token: 0x040056E4 RID: 22244
		private List<int> nodes = new List<int>(128);

		// Token: 0x040056E5 RID: 22245
		private float totalCostInt;

		// Token: 0x040056E6 RID: 22246
		private int curNodeIndex;

		// Token: 0x040056E7 RID: 22247
		public bool inUse;
	}
}
