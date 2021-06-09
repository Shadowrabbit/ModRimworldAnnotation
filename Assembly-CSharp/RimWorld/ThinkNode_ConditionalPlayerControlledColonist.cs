using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4E RID: 3662
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x060052CF RID: 21199 RVA: 0x00039D77 File Offset: 0x00037F77
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
