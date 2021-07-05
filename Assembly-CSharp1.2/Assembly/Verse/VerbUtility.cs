using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B3 RID: 2227
	public static class VerbUtility
	{
		// Token: 0x0600375C RID: 14172 RVA: 0x00160674 File Offset: 0x0015E874
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x00160694 File Offset: 0x0015E894
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

		// Token: 0x0600375E RID: 14174 RVA: 0x001606D4 File Offset: 0x0015E8D4
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x001606F8 File Offset: 0x0015E8F8
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x0016071C File Offset: 0x0015E91C
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0002ADF0 File Offset: 0x00028FF0
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0016073C File Offset: 0x0015E93C
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x00160768 File Offset: 0x0015E968
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

		// Token: 0x06003764 RID: 14180 RVA: 0x001607E4 File Offset: 0x0015E9E4
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

		// Token: 0x06003765 RID: 14181 RVA: 0x0016082C File Offset: 0x0015EA2C
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

		// Token: 0x06003766 RID: 14182 RVA: 0x0002ADFF File Offset: 0x00028FFF
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

		// Token: 0x06003767 RID: 14183 RVA: 0x00160878 File Offset: 0x0015EA78
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x001608B0 File Offset: 0x0015EAB0
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

		// Token: 0x06003769 RID: 14185 RVA: 0x0002AE16 File Offset: 0x00029016
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x0002AE26 File Offset: 0x00029026
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x001608E0 File Offset: 0x0015EAE0
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

		// Token: 0x0600376C RID: 14188 RVA: 0x00160980 File Offset: 0x0015EB80
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

		// Token: 0x020008B4 RID: 2228
		public struct VerbPropertiesWithSource
		{
			// Token: 0x170008B2 RID: 2226
			// (get) Token: 0x0600376D RID: 14189 RVA: 0x0002AE63 File Offset: 0x00029063
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

			// Token: 0x0600376E RID: 14190 RVA: 0x0002AE7A File Offset: 0x0002907A
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x0600376F RID: 14191 RVA: 0x0002AE91 File Offset: 0x00029091
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x04002689 RID: 9865
			public VerbProperties verbProps;

			// Token: 0x0400268A RID: 9866
			public Tool tool;

			// Token: 0x0400268B RID: 9867
			public ManeuverDef maneuver;
		}
	}
}
