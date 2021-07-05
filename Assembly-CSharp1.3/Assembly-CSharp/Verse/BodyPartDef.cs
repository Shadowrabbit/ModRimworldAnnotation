using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000086 RID: 134
	public class BodyPartDef : Def
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00019CF5 File Offset: 0x00017EF5
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00019CFD File Offset: 0x00017EFD
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00019D05 File Offset: 0x00017F05
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00019D21 File Offset: 0x00017F21
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00019D5B File Offset: 0x00017F5B
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

		// Token: 0x060004E6 RID: 1254 RVA: 0x00019D6C File Offset: 0x00017F6C
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x00019DD2 File Offset: 0x00017FD2
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00019DE5 File Offset: 0x00017FE5
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00019DFC File Offset: 0x00017FFC
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

		// Token: 0x040001C6 RID: 454
		[MustTranslate]
		public string labelShort;

		// Token: 0x040001C7 RID: 455
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x040001C8 RID: 456
		public int hitPoints = 10;

		// Token: 0x040001C9 RID: 457
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x040001CA RID: 458
		public float bleedRate = 1f;

		// Token: 0x040001CB RID: 459
		public float frostbiteVulnerability;

		// Token: 0x040001CC RID: 460
		private bool skinCovered;

		// Token: 0x040001CD RID: 461
		private bool solid;

		// Token: 0x040001CE RID: 462
		public bool alive = true;

		// Token: 0x040001CF RID: 463
		public bool delicate;

		// Token: 0x040001D0 RID: 464
		public bool canScarify;

		// Token: 0x040001D1 RID: 465
		public bool beautyRelated;

		// Token: 0x040001D2 RID: 466
		public bool conceptual;

		// Token: 0x040001D3 RID: 467
		public bool socketed;

		// Token: 0x040001D4 RID: 468
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040001D5 RID: 469
		public bool pawnGeneratorCanAmputate;

		// Token: 0x040001D6 RID: 470
		public bool canSuggestAmputation = true;

		// Token: 0x040001D7 RID: 471
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x040001D8 RID: 472
		public bool destroyableByDamage = true;

		// Token: 0x040001D9 RID: 473
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
