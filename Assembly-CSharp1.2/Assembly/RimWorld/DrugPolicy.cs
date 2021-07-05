using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C8 RID: 5320
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06007299 RID: 29337 RVA: 0x0004D12E File Offset: 0x0004B32E
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x1700117C RID: 4476
		public DrugPolicyEntry this[int index]
		{
			get
			{
				return this.entriesInt[index];
			}
			set
			{
				this.entriesInt[index] = value;
			}
		}

		// Token: 0x1700117D RID: 4477
		public DrugPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < this.entriesInt.Count; i++)
				{
					if (this.entriesInt[i].drug == drugDef)
					{
						return this.entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x00006B8B File Offset: 0x00004D8B
		public DrugPolicy()
		{
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x0004D158 File Offset: 0x0004B358
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x00230294 File Offset: 0x0022E494
		public void InitializeIfNeeded()
		{
			if (this.entriesInt != null)
			{
				return;
			}
			this.entriesInt = new List<DrugPolicyEntry>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].category == ThingCategory.Item && allDefsListForReading[i].IsDrug)
				{
					DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
					drugPolicyEntry.drug = allDefsListForReading[i];
					drugPolicyEntry.allowedForAddiction = true;
					this.entriesInt.Add(drugPolicyEntry);
				}
			}
			this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x0023033C File Offset: 0x0022E53C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.entriesInt != null)
			{
				if (this.entriesInt.RemoveAll((DrugPolicyEntry x) => x == null || x.drug == null) != 0)
				{
					Log.Error("Some DrugPolicyEntries were null after loading.", false);
				}
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x0004D174 File Offset: 0x0004B374
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04004B7C RID: 19324
		public int uniqueId;

		// Token: 0x04004B7D RID: 19325
		public string label;

		// Token: 0x04004B7E RID: 19326
		private List<DrugPolicyEntry> entriesInt;
	}
}
