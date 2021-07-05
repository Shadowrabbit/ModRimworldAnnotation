using System;

namespace Verse.AI
{
	// Token: 0x02000613 RID: 1555
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x06002D08 RID: 11528 RVA: 0x0010EF44 File Offset: 0x0010D144
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

		// Token: 0x06002D09 RID: 11529
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x06002D0A RID: 11530 RVA: 0x0010EFE0 File Offset: 0x0010D1E0
		private int GetLastTryTick(Pawn pawn)
		{
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
			{
				return result;
			}
			return -99999;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x0010F00E File Offset: 0x0010D20E
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
