using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DF1 RID: 3569
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x001BAF0C File Offset: 0x001B910C
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04003436 RID: 13366
		public float distToHiveToAttack = 10f;
	}
}
