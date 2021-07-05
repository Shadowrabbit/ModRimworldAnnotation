using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001016 RID: 4118
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060059D9 RID: 23001 RVA: 0x0003E5E1 File Offset: 0x0003C7E1
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x0003E5E9 File Offset: 0x0003C7E9
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x0003E602 File Offset: 0x0003C802
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x0003E61A File Offset: 0x0003C81A
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x001D384C File Offset: 0x001D1A4C
		public AcceptanceReport TryDelete(DrugPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.drugs != null && pawn.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.drugs != null && pawn2.drugs.CurrentPolicy == policy)
				{
					pawn2.drugs.CurrentPolicy = null;
				}
			}
			this.policies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x001D393C File Offset: 0x001D1B3C
		public DrugPolicy MakeNewDrugPolicy()
		{
			int num;
			if (!this.policies.Any<DrugPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.policies.Max((DrugPolicy o) => o.uniqueId) + 1;
			}
			int uniqueId = num;
			DrugPolicy drugPolicy = new DrugPolicy(uniqueId, "DrugPolicy".Translate() + " " + uniqueId.ToString());
			this.policies.Add(drugPolicy);
			return drugPolicy;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x001D39C0 File Offset: 0x001D1BC0
		private void GenerateStartingDrugPolicies()
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = "SocialDrugs".Translate();
			drugPolicy[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
			this.MakeNewDrugPolicy().label = "NoDrugs".Translate();
			DrugPolicy drugPolicy2 = this.MakeNewDrugPolicy();
			drugPolicy2.label = "Unrestricted".Translate();
			for (int i = 0; i < drugPolicy2.Count; i++)
			{
				if (drugPolicy2[i].drug.IsPleasureDrug)
				{
					drugPolicy2[i].allowedForJoy = true;
				}
			}
			DrugPolicy drugPolicy3 = this.MakeNewDrugPolicy();
			drugPolicy3.label = "OneDrinkPerDay".Translate();
			drugPolicy3[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy3[ThingDefOf.Beer].allowScheduled = true;
			drugPolicy3[ThingDefOf.Beer].takeToInventory = 1;
			drugPolicy3[ThingDefOf.Beer].daysFrequency = 1f;
			drugPolicy3[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
		}

		// Token: 0x04003C87 RID: 15495
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
