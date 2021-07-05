using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D72 RID: 3442
	public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
	{
		// Token: 0x06004FDA RID: 20442 RVA: 0x001AB5D8 File Offset: 0x001A97D8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					pawn.stances.stunner.StunFor(base.GetDurationSeconds(pawn).SecondsToTicks(), this.parent.pawn, false, true);
				}
			}
		}
	}
}
