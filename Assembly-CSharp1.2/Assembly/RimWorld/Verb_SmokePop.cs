using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D91 RID: 7569
	public class Verb_SmokePop : Verb
	{
		// Token: 0x0600A474 RID: 42100 RVA: 0x0006D0C5 File Offset: 0x0006B2C5
		protected override bool TryCastShot()
		{
			Verb_SmokePop.Pop(base.ReloadableCompSource);
			return true;
		}

		// Token: 0x0600A475 RID: 42101 RVA: 0x0006D0D3 File Offset: 0x0006B2D3
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return base.EquipmentSource.GetStatValue(StatDefOf.SmokepopBeltRadius, true);
		}

		// Token: 0x0600A476 RID: 42102 RVA: 0x0006D0E9 File Offset: 0x0006B2E9
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x0600A477 RID: 42103 RVA: 0x002FE1C8 File Offset: 0x002FC3C8
		public static void Pop(CompReloadable comp)
		{
			if (comp == null || !comp.CanBeUsed)
			{
				return;
			}
			ThingWithComps parent = comp.parent;
			Pawn wearer = comp.Wearer;
			GenExplosion.DoExplosion(wearer.Position, wearer.Map, parent.GetStatValue(StatDefOf.SmokepopBeltRadius, true), DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
			comp.UsedOnce();
		}
	}
}
