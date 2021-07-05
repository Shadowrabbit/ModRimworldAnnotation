using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1C RID: 3868
	public class RitualStage_InteractWithAnimal : RitualStage
	{
		// Token: 0x06005C0F RID: 23567 RVA: 0x001FC97B File Offset: 0x001FAB7B
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.RaceProps.Animal);
		}
	}
}
