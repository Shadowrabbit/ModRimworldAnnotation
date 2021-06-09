using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B00 RID: 2816
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x06004219 RID: 16921 RVA: 0x00188E98 File Offset: 0x00187098
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

		// Token: 0x0600421A RID: 16922 RVA: 0x00031415 File Offset: 0x0002F615
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
