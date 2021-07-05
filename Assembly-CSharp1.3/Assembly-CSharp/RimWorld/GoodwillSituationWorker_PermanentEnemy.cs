using System;

namespace RimWorld
{
	// Token: 0x02000EC7 RID: 3783
	public class GoodwillSituationWorker_PermanentEnemy : GoodwillSituationWorker
	{
		// Token: 0x06005944 RID: 22852 RVA: 0x001E713D File Offset: 0x001E533D
		public override int GetMaxGoodwill(Faction other)
		{
			if (Faction.OfPlayerSilentFail == null)
			{
				return 100;
			}
			if (!GoodwillSituationWorker_PermanentEnemy.ArePermanentEnemies(Faction.OfPlayer, other))
			{
				return 100;
			}
			return -100;
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x001E715C File Offset: 0x001E535C
		public static bool ArePermanentEnemies(Faction a, Faction b)
		{
			return a.def.permanentEnemy || b.def.permanentEnemy || (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer) || (b.def.permanentEnemyToEveryoneExceptPlayer && !a.IsPlayer) || (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def)) || (b.def.permanentEnemyToEveryoneExcept != null && !b.def.permanentEnemyToEveryoneExcept.Contains(a.def));
		}
	}
}
