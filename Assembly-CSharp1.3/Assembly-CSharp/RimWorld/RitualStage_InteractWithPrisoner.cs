using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1B RID: 3867
	public class RitualStage_InteractWithPrisoner : RitualStage
	{
		// Token: 0x06005C0D RID: 23565 RVA: 0x001FC93D File Offset: 0x001FAB3D
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.IsPrisonerOfColony);
		}
	}
}
