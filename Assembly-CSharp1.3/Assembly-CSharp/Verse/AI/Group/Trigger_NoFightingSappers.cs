using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200069E RID: 1694
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x06002F46 RID: 12102 RVA: 0x00118768 File Offset: 0x00116968
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x001187AF File Offset: 0x001169AF
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
