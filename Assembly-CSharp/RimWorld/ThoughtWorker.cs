using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7C RID: 3708
	public abstract class ThoughtWorker
	{
		// Token: 0x0600533A RID: 21306 RVA: 0x0003A1BA File Offset: 0x000383BA
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x0003A1BA File Offset: 0x000383BA
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0003A1D2 File Offset: 0x000383D2
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x0003A1E1 File Offset: 0x000383E1
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x0003A1F1 File Offset: 0x000383F1
		private ThoughtState PostProcessedState(ThoughtState state)
		{
			if (this.def.invert)
			{
				if (state.Active)
				{
					state = ThoughtState.Inactive;
				}
				else
				{
					state = ThoughtState.ActiveAtStage(0);
				}
			}
			return state;
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x0003A21B File Offset: 0x0003841B
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0003A237 File Offset: 0x00038437
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		// Token: 0x04003508 RID: 13576
		public ThoughtDef def;
	}
}
