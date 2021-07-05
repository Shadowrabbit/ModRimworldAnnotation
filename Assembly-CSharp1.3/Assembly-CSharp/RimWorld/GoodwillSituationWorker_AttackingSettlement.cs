using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC4 RID: 3780
	public class GoodwillSituationWorker_AttackingSettlement : GoodwillSituationWorker
	{
		// Token: 0x0600593A RID: 22842 RVA: 0x001E6FD8 File Offset: 0x001E51D8
		public override int GetMaxGoodwill(Faction other)
		{
			if (!this.IsAttackingSettlement(other))
			{
				return 100;
			}
			return -80;
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x001E6FE8 File Offset: 0x001E51E8
		private bool IsAttackingSettlement(Faction other)
		{
			return Current.ProgramState != ProgramState.Entry && SettlementUtility.IsPlayerAttackingAnySettlementOf(other);
		}

		// Token: 0x04003461 RID: 13409
		private const int MaxGoodwill = -80;
	}
}
