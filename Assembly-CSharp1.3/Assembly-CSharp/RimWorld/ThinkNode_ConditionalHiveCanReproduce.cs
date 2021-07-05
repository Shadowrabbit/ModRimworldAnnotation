using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x06003C86 RID: 15494 RVA: 0x0014F99C File Offset: 0x0014DB9C
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
