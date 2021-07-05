using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F32 RID: 3890
	public abstract class RitualObligationTargetFilter : IExposable
	{
		// Token: 0x06005C7F RID: 23679 RVA: 0x000033AC File Offset: 0x000015AC
		public RitualObligationTargetFilter()
		{
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x001FE1CB File Offset: 0x001FC3CB
		public RitualObligationTargetFilter(RitualObligationTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x001FE1DA File Offset: 0x001FC3DA
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			if (Find.IdeoManager.GetActiveRitualOn(target.Thing) != null)
			{
				return false;
			}
			return this.CanUseTargetInternal(target, obligation);
		}

		// Token: 0x06005C82 RID: 23682
		public abstract IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map);

		// Token: 0x06005C83 RID: 23683
		protected abstract RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation);

		// Token: 0x06005C84 RID: 23684 RVA: 0x00002688 File Offset: 0x00000888
		public virtual List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool ObligationTargetsValid(RitualObligation obligation)
		{
			return true;
		}

		// Token: 0x06005C86 RID: 23686
		public abstract IEnumerable<string> GetTargetInfos(RitualObligation obligation);

		// Token: 0x06005C87 RID: 23687 RVA: 0x001FE203 File Offset: 0x001FC403
		public virtual IEnumerable<string> GetBlockingIssues(TargetInfo target, IEnumerable<Pawn> participants)
		{
			yield break;
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string LabelExtraPart(RitualObligation obligation)
		{
			return "";
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x001FE20C File Offset: 0x001FC40C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualObligationTargetFilterDef>(ref this.def, "def");
			Scribe_References.Look<Precept_Ritual>(ref this.parent, "parent", false);
		}

		// Token: 0x040035D9 RID: 13785
		public Precept_Ritual parent;

		// Token: 0x040035DA RID: 13786
		public RitualObligationTargetFilterDef def;
	}
}
