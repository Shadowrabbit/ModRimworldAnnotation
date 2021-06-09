using System;

namespace Verse
{
	// Token: 0x020003ED RID: 1005
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600188A RID: 6282 RVA: 0x0001747B File Offset: 0x0001567B
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x000DF988 File Offset: 0x000DDB88
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
			}
		}
	}
}
