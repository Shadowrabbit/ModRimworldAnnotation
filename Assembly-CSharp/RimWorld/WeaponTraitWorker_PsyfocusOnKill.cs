using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001011 RID: 4113
	public class WeaponTraitWorker_PsyfocusOnKill : WeaponTraitWorker
	{
		// Token: 0x060059BE RID: 22974 RVA: 0x0003E4DA File Offset: 0x0003C6DA
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

		// Token: 0x04003C66 RID: 15462
		private const float PsyfocusOffset = 0.2f;
	}
}
