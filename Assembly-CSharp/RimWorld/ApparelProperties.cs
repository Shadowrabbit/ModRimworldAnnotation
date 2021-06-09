using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF7 RID: 3831
	public class ApparelProperties
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x060054C9 RID: 21705 RVA: 0x0003AC96 File Offset: 0x00038E96
		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937, false);
				return ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060054CA RID: 21706 RVA: 0x001C6364 File Offset: 0x001C4564
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

		// Token: 0x060054CB RID: 21707 RVA: 0x0003ACD4 File Offset: 0x00038ED4
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || this.gender == wearerGender;
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x001C63D4 File Offset: 0x001C45D4
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x0003ACE9 File Offset: 0x00038EE9
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x00006A05 File Offset: 0x00004C05
		public void PostLoadSpecial(ThingDef thingDef)
		{
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x001C6440 File Offset: 0x001C4640
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

		// Token: 0x060054D0 RID: 21712 RVA: 0x001C6480 File Offset: 0x001C4680
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x0003AD00 File Offset: 0x00038F00
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x001C64D8 File Offset: 0x001C46D8
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

		// Token: 0x04003578 RID: 13688
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04003579 RID: 13689
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x0400357A RID: 13690
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x0400357B RID: 13691
		public WornGraphicData wornGraphicData;

		// Token: 0x0400357C RID: 13692
		public bool useWornGraphicMask;

		// Token: 0x0400357D RID: 13693
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x0400357E RID: 13694
		[NoTranslate]
		public List<string> defaultOutfitTags;

		// Token: 0x0400357F RID: 13695
		public bool canBeGeneratedToSatisfyWarmth = true;

		// Token: 0x04003580 RID: 13696
		public float wearPerDay = 0.4f;

		// Token: 0x04003581 RID: 13697
		public bool careIfWornByCorpse = true;

		// Token: 0x04003582 RID: 13698
		public bool careIfDamaged = true;

		// Token: 0x04003583 RID: 13699
		public bool hatRenderedFrontOfFace;

		// Token: 0x04003584 RID: 13700
		public bool shellRenderedBehindHead;

		// Token: 0x04003585 RID: 13701
		public bool useDeflectMetalEffect;

		// Token: 0x04003586 RID: 13702
		public Gender gender;

		// Token: 0x04003587 RID: 13703
		public float scoreOffset;

		// Token: 0x04003588 RID: 13704
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x04003589 RID: 13705
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		// Token: 0x0400358A RID: 13706
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
