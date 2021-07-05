using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AC7 RID: 2759
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x00030B3B File Offset: 0x0002ED3B
		public override bool IsCaravanSendable
		{
			get
			{
				return this.isCaravanSendable;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x00030B43 File Offset: 0x0002ED43
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00030B53 File Offset: 0x0002ED53
		public LordJob_DefendPoint(IntVec3 point, float? wanderRadius = null, bool isCaravanSendable = false, bool addFleeToil = true)
		{
			this.point = point;
			this.wanderRadius = wanderRadius;
			this.isCaravanSendable = isCaravanSendable;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00030B78 File Offset: 0x0002ED78
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f, this.wanderRadius));
			return stateGraph;
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x001874DC File Offset: 0x001856DC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04002D14 RID: 11540
		private IntVec3 point;

		// Token: 0x04002D15 RID: 11541
		private float? wanderRadius;

		// Token: 0x04002D16 RID: 11542
		private bool isCaravanSendable;

		// Token: 0x04002D17 RID: 11543
		private bool addFleeToil;
	}
}
