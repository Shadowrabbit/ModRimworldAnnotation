using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156A RID: 5482
	public class Thought_Situational : Thought
	{
		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x060076ED RID: 30445 RVA: 0x0005047C File Offset: 0x0004E67C
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x060076EE RID: 30446 RVA: 0x0005048A File Offset: 0x0004E68A
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x060076EF RID: 30447 RVA: 0x00242D94 File Offset: 0x00240F94
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					string text = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						text = this.def.Worker.PostProcessLabel(this.pawn, text);
					}
					return text;
				}
				return base.LabelCap;
			}
		}

		// Token: 0x060076F0 RID: 30448 RVA: 0x00242E1C File Offset: 0x0024101C
		public void RecalculateState()
		{
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
				return;
			}
			this.curStageIndex = -1;
		}

		// Token: 0x060076F1 RID: 30449 RVA: 0x00050492 File Offset: 0x0004E692
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x04004E68 RID: 20072
		private int curStageIndex = -1;

		// Token: 0x04004E69 RID: 20073
		protected string reason;
	}
}
