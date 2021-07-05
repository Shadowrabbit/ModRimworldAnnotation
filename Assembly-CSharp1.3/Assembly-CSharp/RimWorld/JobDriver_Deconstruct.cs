using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DB RID: 1755
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x0011EE02 File Offset: 0x0011D002
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x0011EE09 File Offset: 0x0011D009
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true), 20f, 3000f);
			}
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x0011EE2B File Offset: 0x0011D02B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x0011EE3B File Offset: 0x0011D03B
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x0011EE60 File Offset: 0x0011D060
		protected override void TickAction()
		{
			if (base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f, false);
			}
		}

		// Token: 0x04001D60 RID: 7520
		private const float MaxDeconstructWork = 3000f;

		// Token: 0x04001D61 RID: 7521
		private const float MinDeconstructWork = 20f;
	}
}
