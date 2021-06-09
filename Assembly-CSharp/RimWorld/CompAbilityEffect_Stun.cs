using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001394 RID: 5012
	public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
	{
		// Token: 0x06006CC2 RID: 27842 RVA: 0x002163C8 File Offset: 0x002145C8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					pawn.stances.stunner.StunFor_NewTmp(base.GetDurationSeconds(pawn).SecondsToTicks(), this.parent.pawn, false, true);
				}
			}
		}
	}
}
