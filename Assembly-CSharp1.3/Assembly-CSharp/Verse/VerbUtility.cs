using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F8 RID: 1272
	public static class VerbUtility
	{
		// Token: 0x06002690 RID: 9872 RVA: 0x000EF46C File Offset: 0x000ED66C
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000EF48C File Offset: 0x000ED68C
		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000EF4CC File Offset: 0x000ED6CC
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000EF4F0 File Offset: 0x000ED6F0
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000EF514 File Offset: 0x000ED714
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000EF533 File Offset: 0x000ED733
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000EF544 File Offset: 0x000ED744
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000EF570 File Offset: 0x000ED770
		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000EF5EC File Offset: 0x000ED7EC
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			if (num < 9f)
			{
				return 0f;
			}
			if (num < 25f)
			{
				return forcedMiss * 0.5f;
			}
			if (num < 49f)
			{
				return forcedMiss * 0.8f;
			}
			return forcedMiss;
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000EF634 File Offset: 0x000ED834
		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			if (num <= 25f)
			{
				return 0f;
			}
			if (num >= 144f)
			{
				return 1f;
			}
			return Mathf.InverseLerp(25f, 144f, num);
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000EF680 File Offset: 0x000ED880
		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			if (verbProps != null)
			{
				int num;
				for (int i = 0; i < verbProps.Count; i = num + 1)
				{
					yield return new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
					num = i;
				}
			}
			if (tools != null)
			{
				int num;
				for (int i = 0; i < tools.Count; i = num + 1)
				{
					foreach (ManeuverDef maneuverDef in tools[i].Maneuvers)
					{
						yield return new VerbUtility.VerbPropertiesWithSource(maneuverDef.verb, tools[i], maneuverDef);
					}
					IEnumerator<ManeuverDef> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000EF698 File Offset: 0x000ED898
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000EF6D0 File Offset: 0x000ED8D0
		public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
		{
			float num = VerbUtility.InitialVerbWeight(v, p);
			if (num >= highestWeight * 0.95f)
			{
				return VerbSelectionCategory.Best;
			}
			if (num < highestWeight * 0.25f)
			{
				return VerbSelectionCategory.Worst;
			}
			return VerbSelectionCategory.Mid;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000EF6FE File Offset: 0x000ED8FE
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000EF70E File Offset: 0x000ED90E
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000EF74C File Offset: 0x000ED94C
		private static float AdditionalSelectionFactor(Verb v)
		{
			float num = (v.tool != null) ? v.tool.chanceFactor : 1f;
			if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
			{
				foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
				{
					num += 0.1f;
				}
			}
			return num;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000EF7EC File Offset: 0x000ED9EC
		public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
		{
			VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
			if (selectionCategory == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			int num = 0;
			using (List<Verb>.Enumerator enumerator = allMeleeVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetSelectionCategory(p, highestWeight) == selectionCategory)
					{
						num++;
					}
				}
			}
			return 1f / (float)num * ((selectionCategory == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
		}

		// Token: 0x02001CE4 RID: 7396
		public struct VerbPropertiesWithSource
		{
			// Token: 0x17001A16 RID: 6678
			// (get) Token: 0x0600A884 RID: 43140 RVA: 0x003867D6 File Offset: 0x003849D6
			public ToolCapacityDef ToolCapacity
			{
				get
				{
					if (this.maneuver == null)
					{
						return null;
					}
					return this.maneuver.requiredCapacity;
				}
			}

			// Token: 0x0600A885 RID: 43141 RVA: 0x003867ED File Offset: 0x003849ED
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x0600A886 RID: 43142 RVA: 0x00386804 File Offset: 0x00384A04
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x04006F71 RID: 28529
			public VerbProperties verbProps;

			// Token: 0x04006F72 RID: 28530
			public Tool tool;

			// Token: 0x04006F73 RID: 28531
			public ManeuverDef maneuver;
		}
	}
}
