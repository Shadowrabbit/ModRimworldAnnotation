using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B5A RID: 2906
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06004458 RID: 17496 RVA: 0x0003283A File Offset: 0x00030A3A
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06004459 RID: 17497 RVA: 0x00032841 File Offset: 0x00030A41
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true), 20f, 3000f);
			}
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x00032863 File Offset: 0x00030A63
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

		// Token: 0x0600445B RID: 17499 RVA: 0x00032873 File Offset: 0x00030A73
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x001900D4 File Offset: 0x0018E2D4
		protected override void TickAction()
		{
			if (base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f, false);
			}
		}

		// Token: 0x04002E6B RID: 11883
		private const float MaxDeconstructWork = 3000f;

		// Token: 0x04002E6C RID: 11884
		private const float MinDeconstructWork = 20f;
	}
}
