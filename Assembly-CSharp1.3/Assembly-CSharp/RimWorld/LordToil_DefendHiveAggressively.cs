using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008AD RID: 2221
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x06003ABC RID: 15036 RVA: 0x00148784 File Offset: 0x00146984
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

		// Token: 0x04002019 RID: 8217
		public float distToHiveToAttack = 40f;
	}
}
