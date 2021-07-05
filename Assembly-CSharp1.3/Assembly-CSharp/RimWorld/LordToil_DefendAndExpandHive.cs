using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008AB RID: 2219
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x06003AB7 RID: 15031 RVA: 0x00148684 File Offset: 0x00146884
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

		// Token: 0x04002017 RID: 8215
		public float distToHiveToAttack = 10f;
	}
}
