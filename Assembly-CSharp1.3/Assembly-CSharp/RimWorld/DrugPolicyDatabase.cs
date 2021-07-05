using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEF RID: 2799
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x00160B86 File Offset: 0x0015ED86
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x00160B8E File Offset: 0x0015ED8E
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x00160BA7 File Offset: 0x0015EDA7
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x00160BBF File Offset: 0x0015EDBF
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x00160BE4 File Offset: 0x0015EDE4
		public void MakePolicyDefault(DrugPolicyDef policyDef)
		{
			if (this.DefaultDrugPolicy().sourceDef == policyDef)
			{
				return;
			}
			DrugPolicy drugPolicy = this.policies.FirstOrDefault((DrugPolicy x) => x.sourceDef == policyDef);
			if (drugPolicy != null)
			{
				this.policies.Remove(drugPolicy);
				this.policies.Insert(0, drugPolicy);
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x00160C48 File Offset: 0x0015EE48
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

		// Token: 0x060041DC RID: 16860 RVA: 0x00160D38 File Offset: 0x0015EF38
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

		// Token: 0x060041DD RID: 16861 RVA: 0x00160DBC File Offset: 0x0015EFBC
		public DrugPolicy NewDrugPolicyFromDef(DrugPolicyDef def)
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = def.LabelCap;
			drugPolicy.sourceDef = def;
			if (def.allowPleasureDrugs)
			{
				for (int i = 0; i < drugPolicy.Count; i++)
				{
					if (drugPolicy[i].drug.IsPleasureDrug)
					{
						drugPolicy[i].allowedForJoy = true;
					}
				}
			}
			if (def.entries != null)
			{
				for (int j = 0; j < def.entries.Count; j++)
				{
					drugPolicy[def.entries[j].drug].CopyFrom(def.entries[j]);
				}
			}
			return drugPolicy;
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x00160E68 File Offset: 0x0015F068
		private void GenerateStartingDrugPolicies()
		{
			foreach (DrugPolicyDef def in DefDatabase<DrugPolicyDef>.AllDefs)
			{
				this.NewDrugPolicyFromDef(def);
			}
		}

		// Token: 0x0400282A RID: 10282
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
