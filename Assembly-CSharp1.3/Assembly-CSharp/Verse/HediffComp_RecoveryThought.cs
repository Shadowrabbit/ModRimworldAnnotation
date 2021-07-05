using System;

namespace Verse
{
	// Token: 0x020002B0 RID: 688
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x0006B8F5 File Offset: 0x00069AF5
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0006B904 File Offset: 0x00069B04
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
			}
		}
	}
}
