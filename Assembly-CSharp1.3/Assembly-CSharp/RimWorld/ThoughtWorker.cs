using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
	public abstract class ThoughtWorker
	{
		// Token: 0x06003CAB RID: 15531 RVA: 0x0014FEDE File Offset: 0x0014E0DE
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x0014FEDE File Offset: 0x0014E0DE
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x0014FEF6 File Offset: 0x0014E0F6
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x0014FF05 File Offset: 0x0014E105
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x0014FF15 File Offset: 0x0014E115
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

		// Token: 0x06003CB0 RID: 15536 RVA: 0x0014FF3F File Offset: 0x0014E13F
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0014FF5B File Offset: 0x0014E15B
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		// Token: 0x040020B8 RID: 8376
		public ThoughtDef def;
	}
}
