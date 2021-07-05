using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E67 RID: 3687
	public static class ApparelUtility
	{
		// Token: 0x0600558A RID: 21898 RVA: 0x001CF714 File Offset: 0x001CD914
		public static bool IsRequirementActive(ApparelRequirement requirement, ApparelRequirementSource source, Pawn pawn, out string disabledByLabel)
		{
			bool flag = false;
			if (pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Nudist))
			{
				flag = true;
			}
			else if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersNudity())
			{
				flag = true;
			}
			if (flag)
			{
				disabledByLabel = "Nudism".Translate();
				return false;
			}
			if (source != ApparelRequirementSource.Title && pawn.royalty != null && !pawn.royalty.AllTitlesForReading.NullOrEmpty<RoyalTitle>())
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
					{
						disabledByLabel = royalTitle.def.GetLabelCapFor(pawn);
						return false;
					}
				}
			}
			disabledByLabel = null;
			return true;
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x001CF80C File Offset: 0x001CDA0C
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
			List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
			BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
			BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
			for (int k = 0; k < bodyPartGroups.Count; k++)
			{
				if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
				{
					return false;
				}
			}
			for (int l = 0; l < bodyPartGroups2.Count; l++)
			{
				if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x001CF910 File Offset: 0x001CDB10
		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x001CF974 File Offset: 0x001CDB74
		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			ApparelUtility.<>c__DisplayClass4_0 CS$<>8__locals1 = new ApparelUtility.<>c__DisplayClass4_0();
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			bool flag = false;
			for (int j = 0; j < hediffs.Count; j++)
			{
				if (hediffs[j] is Hediff_MissingPart)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			CS$<>8__locals1.groups = apparel.apparel.bodyPartGroups;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.groups.Count; i = i2 + 1)
			{
				if (notMissingParts.Any((BodyPartRecord x) => x.IsInGroup(CS$<>8__locals1.groups[i])))
				{
					return true;
				}
				i2 = i;
			}
			return false;
		}

		// Token: 0x020022BF RID: 8895
		public struct LayerGroupPair : IEquatable<ApparelUtility.LayerGroupPair>
		{
			// Token: 0x0600C440 RID: 50240 RVA: 0x003DAD5C File Offset: 0x003D8F5C
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x0600C441 RID: 50241 RVA: 0x003DAD6C File Offset: 0x003D8F6C
			public override bool Equals(object rhs)
			{
				return rhs is ApparelUtility.LayerGroupPair && this.Equals((ApparelUtility.LayerGroupPair)rhs);
			}

			// Token: 0x0600C442 RID: 50242 RVA: 0x003DAD84 File Offset: 0x003D8F84
			public bool Equals(ApparelUtility.LayerGroupPair other)
			{
				return other.layer == this.layer && other.group == this.group;
			}

			// Token: 0x0600C443 RID: 50243 RVA: 0x003DADA4 File Offset: 0x003D8FA4
			public override int GetHashCode()
			{
				return (17 * 23 + this.layer.GetHashCode()) * 23 + this.group.GetHashCode();
			}

			// Token: 0x0400849B RID: 33947
			private readonly ApparelLayerDef layer;

			// Token: 0x0400849C RID: 33948
			private readonly BodyPartGroupDef group;
		}
	}
}
