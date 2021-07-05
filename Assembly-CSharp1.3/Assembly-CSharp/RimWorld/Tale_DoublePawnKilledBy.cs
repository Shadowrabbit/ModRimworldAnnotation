using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001034 RID: 4148
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x060061F9 RID: 25081 RVA: 0x00214719 File Offset: 0x00212919
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x00214828 File Offset: 0x00212A28
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
