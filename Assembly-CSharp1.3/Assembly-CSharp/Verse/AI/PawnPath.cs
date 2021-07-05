using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000601 RID: 1537
	public class PawnPath : IDisposable
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x00107149 File Offset: 0x00105349
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002C29 RID: 11305 RVA: 0x0010715B File Offset: 0x0010535B
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x00107163 File Offset: 0x00105363
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x0010716D File Offset: 0x0010536D
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x00107181 File Offset: 0x00105381
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x00107189 File Offset: 0x00105389
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x00107191 File Offset: 0x00105391
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x001071AB File Offset: 0x001053AB
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x001071B9 File Offset: 0x001053B9
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x001071C0 File Offset: 0x001053C0
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x001071D0 File Offset: 0x001053D0
		public void SetupFound(float totalCost, bool usedRegionHeuristics)
		{
			if (this == PawnPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on PawnPath.NotFound");
				return;
			}
			this.totalCostInt = totalCost;
			this.usedRegionHeuristics = usedRegionHeuristics;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x00107221 File Offset: 0x00105421
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x00107229 File Offset: 0x00105429
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

		// Token: 0x06002C35 RID: 11317 RVA: 0x00107257 File Offset: 0x00105457
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00107269 File Offset: 0x00105469
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00107280 File Offset: 0x00105480
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x00107298 File Offset: 0x00105498
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

		// Token: 0x06002C39 RID: 11321 RVA: 0x00107354 File Offset: 0x00105554
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

		// Token: 0x04001ADF RID: 6879
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x04001AE0 RID: 6880
		private float totalCostInt;

		// Token: 0x04001AE1 RID: 6881
		private int curNodeIndex;

		// Token: 0x04001AE2 RID: 6882
		private bool usedRegionHeuristics;

		// Token: 0x04001AE3 RID: 6883
		public bool inUse;
	}
}
