using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200027B RID: 635
	public class HediffComp
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0006913D File Offset: 0x0006733D
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0006914A File Offset: 0x0006734A
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x000683B1 File Offset: 0x000665B1
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompExposeData()
		{
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> CompGetGizmos()
		{
			return null;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x04000DC3 RID: 3523
		public HediffWithComps parent;

		// Token: 0x04000DC4 RID: 3524
		public HediffCompProperties props;
	}
}
