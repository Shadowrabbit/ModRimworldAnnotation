using System;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x06003FEB RID: 16363 RVA: 0x00181B64 File Offset: 0x0017FD64
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
			{
				return ThinkResult.NoJob;
			}
			this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
			float num = this.MtbHours(pawn);
			if (num <= 0f)
			{
				return ThinkResult.NoJob;
			}
			Rand.PushState();
			int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
			Rand.Seed = pawn.RandSeedForHour(salt);
			bool flag = Rand.MTBEventOccurs(num, 2500f, 2500f);
			Rand.PopState();
			if (flag)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06003FEC RID: 16364
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x06003FED RID: 16365 RVA: 0x00181C00 File Offset: 0x0017FE00
		private int GetLastTryTick(Pawn pawn)
		{
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
			{
				return result;
			}
			return -99999;
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0002FE14 File Offset: 0x0002E014
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
