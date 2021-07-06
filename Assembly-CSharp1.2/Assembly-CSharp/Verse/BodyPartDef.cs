using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E3 RID: 227
	public class BodyPartDef : Def
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0000B88C File Offset: 0x00009A8C
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0000B894 File Offset: 0x00009A94
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0000B89C File Offset: 0x00009A9C
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return base.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0000B8F2 File Offset: 0x00009AF2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00090124 File Offset: 0x0008E324
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0000B902 File Offset: 0x00009B02
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000B915 File Offset: 0x00009B15
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0009018C File Offset: 0x0008E38C
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.conceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x04000388 RID: 904
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000389 RID: 905
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x0400038A RID: 906
		public int hitPoints = 10;

		// Token: 0x0400038B RID: 907
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x0400038C RID: 908
		public float bleedRate = 1f;

		// Token: 0x0400038D RID: 909
		public float frostbiteVulnerability;

		// Token: 0x0400038E RID: 910
		private bool skinCovered;

		// Token: 0x0400038F RID: 911
		private bool solid;

		// Token: 0x04000390 RID: 912
		public bool alive = true;

		// Token: 0x04000391 RID: 913
		public bool delicate;

		// Token: 0x04000392 RID: 914
		public bool beautyRelated;

		// Token: 0x04000393 RID: 915
		public bool conceptual;

		// Token: 0x04000394 RID: 916
		public bool socketed;

		// Token: 0x04000395 RID: 917
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000396 RID: 918
		public bool pawnGeneratorCanAmputate;

		// Token: 0x04000397 RID: 919
		public bool canSuggestAmputation = true;

		// Token: 0x04000398 RID: 920
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x04000399 RID: 921
		public bool destroyableByDamage = true;

		// Token: 0x0400039A RID: 922
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
