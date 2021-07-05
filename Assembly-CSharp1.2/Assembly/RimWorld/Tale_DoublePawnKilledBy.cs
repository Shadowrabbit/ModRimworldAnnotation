using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001653 RID: 5715
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x06007C6E RID: 31854 RVA: 0x00053783 File Offset: 0x00051983
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x06007C6F RID: 31855 RVA: 0x0005390F File Offset: 0x00051B0F
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
