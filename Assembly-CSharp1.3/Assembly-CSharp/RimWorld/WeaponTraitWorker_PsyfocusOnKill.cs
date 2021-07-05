using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEB RID: 2795
	public class WeaponTraitWorker_PsyfocusOnKill : WeaponTraitWorker
	{
		// Token: 0x060041C5 RID: 16837 RVA: 0x001606E0 File Offset: 0x0015E8E0
		public override void Notify_KilledPawn(Pawn pawn)
		{
			base.Notify_KilledPawn(pawn);
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(0.2f);
		}

		// Token: 0x0400280F RID: 10255
		private const float PsyfocusOffset = 0.2f;
	}
}
