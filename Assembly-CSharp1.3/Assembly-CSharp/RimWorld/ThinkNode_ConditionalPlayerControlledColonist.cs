using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000905 RID: 2309
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x06003C3B RID: 15419 RVA: 0x0014F35D File Offset: 0x0014D55D
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
