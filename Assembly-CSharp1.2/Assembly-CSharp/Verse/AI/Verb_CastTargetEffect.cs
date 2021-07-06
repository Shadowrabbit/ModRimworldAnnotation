using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000965 RID: 2405
	public class Verb_CastTargetEffect : Verb_CastBase
	{
		// Token: 0x06003ADE RID: 15070 RVA: 0x0016BCBC File Offset: 0x00169EBC
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			Thing thing = this.currentTarget.Thing;
			if (casterPawn == null || thing == null)
			{
				return false;
			}
			foreach (CompTargetEffect compTargetEffect in base.EquipmentSource.GetComps<CompTargetEffect>())
			{
				compTargetEffect.DoEffectOn(casterPawn, thing);
			}
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
