using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1D RID: 3869
	public class RitualStage_OnTheThrone : RitualStage
	{
		// Token: 0x06005C11 RID: 23569 RVA: 0x001FC9B1 File Offset: 0x001FABB1
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.GetFirstThing(ritual.Map);
		}
	}
}
