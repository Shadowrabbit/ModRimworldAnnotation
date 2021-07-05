using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001519 RID: 5401
	public class Verb_SmokePop : Verb
	{
		// Token: 0x06008094 RID: 32916 RVA: 0x002D8FCD File Offset: 0x002D71CD
		protected override bool TryCastShot()
		{
			Verb_SmokePop.Pop(base.ReloadableCompSource);
			return true;
		}

		// Token: 0x06008095 RID: 32917 RVA: 0x002D8FDB File Offset: 0x002D71DB
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return base.EquipmentSource.GetStatValue(StatDefOf.SmokepopBeltRadius, true);
		}

		// Token: 0x06008096 RID: 32918 RVA: 0x002D8FF1 File Offset: 0x002D71F1
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x06008097 RID: 32919 RVA: 0x002D9004 File Offset: 0x002D7204
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
