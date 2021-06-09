using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E42 RID: 3650
	public class ThinkNode_Priority_GetJoy : ThinkNode_Priority
	{
		// Token: 0x060052B7 RID: 21175 RVA: 0x001BFA60 File Offset: 0x001BDC60
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				return 0f;
			}
			float curLevel = pawn.needs.joy.CurLevel;
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (!timeAssignmentDef.allowJoy)
			{
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.35f)
				{
					return 6f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				if (curLevel < 0.95f)
				{
					return 7f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				if (curLevel < 0.95f)
				{
					return 2f;
				}
				return 0f;
			}
			else
			{
				if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
				{
					return 0f;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x040034F7 RID: 13559
		private const int GameStartNoJoyTicks = 5000;
	}
}
