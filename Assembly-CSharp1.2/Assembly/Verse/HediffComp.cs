using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003BB RID: 955
	public class HediffComp
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060017C0 RID: 6080 RVA: 0x00016B49 File Offset: 0x00014D49
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00016B56 File Offset: 0x00014D56
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x000166B2 File Offset: 0x000148B2
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostMake()
		{
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompExposeData()
		{
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompTended_NewTemp(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x04001220 RID: 4640
		public HediffWithComps parent;

		// Token: 0x04001221 RID: 4641
		public HediffCompProperties props;
	}
}
