using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2C RID: 3628
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060053DF RID: 21471 RVA: 0x001C6268 File Offset: 0x001C4468
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x17000E44 RID: 3652
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

		// Token: 0x17000E45 RID: 3653
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

		// Token: 0x060053E3 RID: 21475 RVA: 0x000033AC File Offset: 0x000015AC
		public DrugPolicy()
		{
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x001C62DD File Offset: 0x001C44DD
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded(true);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x001C62FC File Offset: 0x001C44FC
		public void InitializeIfNeeded(bool overwriteExisting = true)
		{
			DrugPolicy.<>c__DisplayClass13_0 CS$<>8__locals1 = new DrugPolicy.<>c__DisplayClass13_0();
			if (overwriteExisting)
			{
				if (this.entriesInt != null)
				{
					return;
				}
				this.entriesInt = new List<DrugPolicyEntry>();
			}
			CS$<>8__locals1.thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.thingDefs.Count; i = j + 1)
			{
				if (CS$<>8__locals1.thingDefs[i].category == ThingCategory.Item && CS$<>8__locals1.thingDefs[i].IsDrug && (overwriteExisting || !this.entriesInt.Any((DrugPolicyEntry x) => x.drug == CS$<>8__locals1.thingDefs[i])))
				{
					DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
					drugPolicyEntry.drug = CS$<>8__locals1.thingDefs[i];
					drugPolicyEntry.allowedForAddiction = true;
					this.entriesInt.Add(drugPolicyEntry);
				}
				j = i;
			}
			this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x001C642C File Offset: 0x001C462C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<DrugPolicyDef>(ref this.sourceDef, "sourceDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.entriesInt != null)
			{
				if (this.entriesInt.RemoveAll((DrugPolicyEntry x) => x == null || x.drug == null) != 0)
				{
					Log.Error("Some DrugPolicyEntries were null after loading.");
				}
				this.InitializeIfNeeded(false);
			}
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x001C64D0 File Offset: 0x001C46D0
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04003160 RID: 12640
		public int uniqueId;

		// Token: 0x04003161 RID: 12641
		public string label;

		// Token: 0x04003162 RID: 12642
		public DrugPolicyDef sourceDef;

		// Token: 0x04003163 RID: 12643
		private List<DrugPolicyEntry> entriesInt;
	}
}
