using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E41 RID: 3649
	public class ThinkNode_JoinVoluntarilyJoinableLord : ThinkNode_Priority
	{
		// Token: 0x060052B2 RID: 21170 RVA: 0x00039CA6 File Offset: 0x00037EA6
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JoinVoluntarilyJoinableLord thinkNode_JoinVoluntarilyJoinableLord = (ThinkNode_JoinVoluntarilyJoinableLord)base.DeepCopy(resolve);
			thinkNode_JoinVoluntarilyJoinableLord.dutyHook = this.dutyHook;
			return thinkNode_JoinVoluntarilyJoinableLord;
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x001BF8D8 File Offset: 0x001BDAD8
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

		// Token: 0x060052B4 RID: 21172 RVA: 0x001BF934 File Offset: 0x001BDB34
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

		// Token: 0x060052B5 RID: 21173 RVA: 0x001BF97C File Offset: 0x001BDB7C
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

		// Token: 0x040034F6 RID: 13558
		public ThinkTreeDutyHook dutyHook;
	}
}
