using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public static class TooltipUtility
	{
		// Token: 0x06002003 RID: 8195 RVA: 0x000C6258 File Offset: 0x000C4458
		public static string ShotCalculationTipString(Thing target)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Find.Selector.SingleSelectedThing != null)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				Verb verb = null;
				Pawn pawn = singleSelectedThing as Pawn;
				if (pawn != null && pawn != target && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.PrimaryEq.PrimaryVerb is Verb_LaunchProjectile)
				{
					verb = pawn.equipment.PrimaryEq.PrimaryVerb;
				}
				Building_TurretGun building_TurretGun = singleSelectedThing as Building_TurretGun;
				if (building_TurretGun != null && building_TurretGun != target)
				{
					verb = building_TurretGun.AttackVerb;
				}
				if (verb != null)
				{
					stringBuilder.Append("ShotBy".Translate(Find.Selector.SingleSelectedThing.LabelShort, Find.Selector.SingleSelectedThing) + ": ");
					if (verb.CanHitTarget(target))
					{
						stringBuilder.Append(ShotReport.HitReportFor(verb.caster, verb, target).GetTextReadout());
					}
					else
					{
						stringBuilder.AppendLine("CannotHit".Translate());
					}
					Pawn pawn2 = target as Pawn;
					if (pawn2 != null && pawn2.Faction == null && !pawn2.InAggroMentalState && pawn2.AnimalOrWildMan())
					{
						float manhunterOnDamageChance;
						if (verb.IsMeleeAttack)
						{
							manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn2, 0f, singleSelectedThing);
						}
						else
						{
							manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn2, singleSelectedThing);
						}
						if (manhunterOnDamageChance > 0f)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(string.Format("{0}: {1}", "ManhunterPerHit".Translate(), manhunterOnDamageChance.ToStringPercent()));
						}
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
