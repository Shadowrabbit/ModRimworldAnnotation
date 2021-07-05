using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008F8 RID: 2296
	public class ThinkNode_JoinVoluntarilyJoinableLord : ThinkNode_Priority
	{
		// Token: 0x06003C1E RID: 15390 RVA: 0x0014EF9C File Offset: 0x0014D19C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JoinVoluntarilyJoinableLord thinkNode_JoinVoluntarilyJoinableLord = (ThinkNode_JoinVoluntarilyJoinableLord)base.DeepCopy(resolve);
			thinkNode_JoinVoluntarilyJoinableLord.dutyHook = this.dutyHook;
			return thinkNode_JoinVoluntarilyJoinableLord;
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x0014EFB8 File Offset: 0x0014D1B8
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			this.CheckLeaveCurrentVoluntarilyJoinableLord(pawn);
			this.JoinVoluntarilyJoinableLord(pawn);
			if (pawn.GetLord() != null && (pawn.mindState.duty == null || pawn.mindState.duty.def.hook == this.dutyHook))
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0014F014 File Offset: 0x0014D214
		private void CheckLeaveCurrentVoluntarilyJoinableLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable = lord.LordJob as LordJob_VoluntarilyJoinable;
			if (lordJob_VoluntarilyJoinable == null)
			{
				return;
			}
			if (lordJob_VoluntarilyJoinable.VoluntaryJoinPriorityFor(pawn) <= 0f)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x0014F05C File Offset: 0x0014D25C
		private void JoinVoluntarilyJoinableLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			Lord lord2 = null;
			float num = 0f;
			if (lord != null)
			{
				LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable = lord.LordJob as LordJob_VoluntarilyJoinable;
				if (lordJob_VoluntarilyJoinable == null)
				{
					return;
				}
				lord2 = lord;
				num = lordJob_VoluntarilyJoinable.VoluntaryJoinPriorityFor(pawn);
			}
			List<Lord> lords = pawn.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable2 = lords[i].LordJob as LordJob_VoluntarilyJoinable;
				if (lordJob_VoluntarilyJoinable2 != null && lords[i].CurLordToil.VoluntaryJoinDutyHookFor(pawn) == this.dutyHook)
				{
					float num2 = lordJob_VoluntarilyJoinable2.VoluntaryJoinPriorityFor(pawn);
					if (num2 > 0f && (lord2 == null || num2 > num))
					{
						lord2 = lords[i];
						num = num2;
					}
				}
			}
			if (lord2 != null && lord != lord2)
			{
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
				}
				lord2.AddPawn(pawn);
			}
		}

		// Token: 0x040020A8 RID: 8360
		public ThinkTreeDutyHook dutyHook;
	}
}
