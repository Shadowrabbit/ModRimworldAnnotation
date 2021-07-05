using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA1 RID: 3745
	public class Thought_Situational : Thought
	{
		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005803 RID: 22531 RVA: 0x001DE7CA File Offset: 0x001DC9CA
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005804 RID: 22532 RVA: 0x001DE7D8 File Offset: 0x001DC9D8
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005805 RID: 22533 RVA: 0x001DE7E0 File Offset: 0x001DC9E0
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

		// Token: 0x06005806 RID: 22534 RVA: 0x001DE868 File Offset: 0x001DCA68
		public void RecalculateState()
		{
			bool active = this.Active;
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
			}
			else
			{
				this.curStageIndex = -1;
			}
			bool active2 = this.Active;
			if (active && !active2)
			{
				this.Notify_BecameInactive();
			}
			if (!active && active2)
			{
				this.Notify_BecameActive();
			}
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x001DE8DC File Offset: 0x001DCADC
		protected virtual void Notify_BecameActive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.RemoveMemoriesOfDef(this.def.producesMemoryThought);
			}
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x001DE92C File Offset: 0x001DCB2C
		protected virtual void Notify_BecameInactive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.TryGainMemory(this.def.producesMemoryThought, null, this.sourcePrecept);
			}
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x001DE981 File Offset: 0x001DCB81
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x040033DB RID: 13275
		private int curStageIndex = -1;

		// Token: 0x040033DC RID: 13276
		protected string reason;
	}
}
