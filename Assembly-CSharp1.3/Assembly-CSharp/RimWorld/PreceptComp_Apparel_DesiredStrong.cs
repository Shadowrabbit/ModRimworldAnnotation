using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE9 RID: 3817
	public class PreceptComp_Apparel_DesiredStrong : PreceptComp_Apparel
	{
		// Token: 0x06005AA2 RID: 23202 RVA: 0x001F587C File Offset: 0x001F3A7C
		public override void Notify_MemberGenerated(Pawn pawn, Precept precept)
		{
			if (!base.AppliesToPawn(pawn, precept))
			{
				return;
			}
			base.GiveApparelToPawn(pawn, (Precept_Apparel)precept);
		}
	}
}
