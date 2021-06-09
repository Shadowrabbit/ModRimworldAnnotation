using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E0E RID: 3598
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x060051CE RID: 20942 RVA: 0x001BCD38 File Offset: 0x001BAF38
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
