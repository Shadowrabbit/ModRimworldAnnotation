using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009ED RID: 2541
	public class ApparelProperties
	{
		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003E93 RID: 16019 RVA: 0x001566FC File Offset: 0x001548FC
		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937);
				return ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003E94 RID: 16020 RVA: 0x0015673C File Offset: 0x0015493C
		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x001567AB File Offset: 0x001549AB
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || this.gender == wearerGender;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001567C0 File Offset: 0x001549C0
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x00156829 File Offset: 0x00154A29
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0000313F File Offset: 0x0000133F
		public void PostLoadSpecial(ThingDef thingDef)
		{
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00156840 File Offset: 0x00154A40
		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00156880 File Offset: 0x00154A80
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x001568D9 File Offset: 0x00154AD9
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x00156914 File Offset: 0x00154B14
		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartGroupDef[] array = (from bpgd in (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>().SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		// Token: 0x040020FD RID: 8445
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x040020FE RID: 8446
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x040020FF RID: 8447
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x04002100 RID: 8448
		[NoTranslate]
		public List<string> wornGraphicPaths;

		// Token: 0x04002101 RID: 8449
		public WornGraphicData wornGraphicData;

		// Token: 0x04002102 RID: 8450
		public bool useWornGraphicMask;

		// Token: 0x04002103 RID: 8451
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04002104 RID: 8452
		[NoTranslate]
		public List<string> defaultOutfitTags;

		// Token: 0x04002105 RID: 8453
		public bool canBeGeneratedToSatisfyWarmth = true;

		// Token: 0x04002106 RID: 8454
		public bool canBeDesiredForIdeo = true;

		// Token: 0x04002107 RID: 8455
		public float wearPerDay = 0.4f;

		// Token: 0x04002108 RID: 8456
		public bool careIfWornByCorpse = true;

		// Token: 0x04002109 RID: 8457
		public bool careIfDamaged = true;

		// Token: 0x0400210A RID: 8458
		public bool ignoredByNonViolent;

		// Token: 0x0400210B RID: 8459
		public bool ai_pickUpOpportunistically;

		// Token: 0x0400210C RID: 8460
		public bool hatRenderedFrontOfFace;

		// Token: 0x0400210D RID: 8461
		public bool hatRenderedAboveBody;

		// Token: 0x0400210E RID: 8462
		public bool hatRenderedBehindHead;

		// Token: 0x0400210F RID: 8463
		public bool shellRenderedBehindHead;

		// Token: 0x04002110 RID: 8464
		public bool shellCoversHead;

		// Token: 0x04002111 RID: 8465
		public bool blocksVision;

		// Token: 0x04002112 RID: 8466
		public bool slaveApparel;

		// Token: 0x04002113 RID: 8467
		public bool legsNakedUnlessCoveredBySomethingElse;

		// Token: 0x04002114 RID: 8468
		public bool useDeflectMetalEffect;

		// Token: 0x04002115 RID: 8469
		public bool countsAsClothingForNudity = true;

		// Token: 0x04002116 RID: 8470
		public Gender gender;

		// Token: 0x04002117 RID: 8471
		public float scoreOffset;

		// Token: 0x04002118 RID: 8472
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x04002119 RID: 8473
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		// Token: 0x0400211A RID: 8474
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
