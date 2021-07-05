using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCB RID: 4043
	public class CompRitualEffect_SpawnOnRole : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06005F26 RID: 24358 RVA: 0x00208C2C File Offset: 0x00206E2C
		protected new CompProperties_RitualEffectSpawnOnRole Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnRole)this.props;
			}
		}

		// Token: 0x06005F27 RID: 24359 RVA: 0x00208C39 File Offset: 0x00206E39
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.PawnWithRole(this.Props.roleId);
		}
	}
}
