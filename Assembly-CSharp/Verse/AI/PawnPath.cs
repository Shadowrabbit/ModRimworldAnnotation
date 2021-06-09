using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A59 RID: 2649
	public class PawnPath : IDisposable
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06003EFB RID: 16123 RVA: 0x0002F4D9 File Offset: 0x0002D6D9
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0002F4EB File Offset: 0x0002D6EB
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06003EFD RID: 16125 RVA: 0x0002F4F3 File Offset: 0x0002D6F3
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x0002F4FD File Offset: 0x0002D6FD
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003EFF RID: 16127 RVA: 0x0002F511 File Offset: 0x0002D711
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06003F00 RID: 16128 RVA: 0x0002F519 File Offset: 0x0002D719
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06003F01 RID: 16129 RVA: 0x0002F521 File Offset: 0x0002D721
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06003F02 RID: 16130 RVA: 0x0002F53B File Offset: 0x0002D73B
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003F03 RID: 16131 RVA: 0x0002F549 File Offset: 0x0002D749
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0002F550 File Offset: 0x0002D750
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x0017B910 File Offset: 0x00179B10
		public void SetupFound(float totalCost, bool usedRegionHeuristics)
		{
			if (this == PawnPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on PawnPath.NotFound", false);
				return;
			}
			this.totalCostInt = totalCost;
			this.usedRegionHeuristics = usedRegionHeuristics;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x0002F55E File Offset: 0x0002D75E
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0002F566 File Offset: 0x0002D766
		public void ReleaseToPool()
		{
			if (this != PawnPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.usedRegionHeuristics = false;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0002F594 File Offset: 0x0002D794
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0002F5A6 File Offset: 0x0002D7A6
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0002F5BD File Offset: 0x0002D7BD
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0017B964 File Offset: 0x00179B64
		public override string ToString()
		{
			if (!this.Found)
			{
				return "PawnPath(not found)";
			}
			if (!this.inUse)
			{
				return "PawnPath(not in use)";
			}
			return string.Concat(new object[]
			{
				"PawnPath(nodeCount= ",
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

		// Token: 0x06003F0C RID: 16140 RVA: 0x0017BA20 File Offset: 0x00179C20
		public void DrawPath(Pawn pathingPawn)
		{
			if (!this.Found)
			{
				return;
			}
			float y = AltitudeLayer.Item.AltitudeFor();
			if (this.NodesLeftCount > 0)
			{
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = this.Peek(i).ToVector3Shifted();
					a.y = y;
					Vector3 b = this.Peek(i + 1).ToVector3Shifted();
					b.y = y;
					GenDraw.DrawLineBetween(a, b);
				}
				if (pathingPawn != null)
				{
					Vector3 drawPos = pathingPawn.DrawPos;
					drawPos.y = y;
					Vector3 b2 = this.Peek(0).ToVector3Shifted();
					b2.y = y;
					if ((drawPos - b2).sqrMagnitude > 0.01f)
					{
						GenDraw.DrawLineBetween(drawPos, b2);
					}
				}
			}
		}

		// Token: 0x04002B64 RID: 11108
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x04002B65 RID: 11109
		private float totalCostInt;

		// Token: 0x04002B66 RID: 11110
		private int curNodeIndex;

		// Token: 0x04002B67 RID: 11111
		private bool usedRegionHeuristics;

		// Token: 0x04002B68 RID: 11112
		public bool inUse;
	}
}
