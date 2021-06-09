using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E72 RID: 3698
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x0600531D RID: 21277 RVA: 0x001BFE24 File Offset: 0x001BE024
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
