using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DF3 RID: 3571
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x06005153 RID: 20819 RVA: 0x001BAFE8 File Offset: 0x001B91E8
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04003438 RID: 13368
		public float distToHiveToAttack = 40f;
	}
}
