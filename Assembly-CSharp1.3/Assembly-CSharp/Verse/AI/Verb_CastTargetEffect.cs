using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000589 RID: 1417
	public class Verb_CastTargetEffect : Verb_CastBase
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x000FAF78 File Offset: 0x000F9178
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
