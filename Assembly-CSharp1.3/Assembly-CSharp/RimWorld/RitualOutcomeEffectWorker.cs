using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6C RID: 3948
	public abstract class RitualOutcomeEffectWorker : IExposable
	{
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06005D9A RID: 23962 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ApplyOnFailure
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06005D9B RID: 23963 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x000033AC File Offset: 0x000015AC
		public RitualOutcomeEffectWorker()
		{
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x0020172D File Offset: 0x001FF92D
		public RitualOutcomeEffectWorker(RitualOutcomeEffectDef def)
		{
			this.def = def;
			this.FillCompData();
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x00201744 File Offset: 0x001FF944
		public void FillCompData()
		{
			if ((this.compDatas == null || this.compDatas.Count != this.def.comps.Count) && !this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				this.compDatas = new List<RitualOutcomeComp_Data>();
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					this.compDatas.Add(ritualOutcomeComp.MakeData());
				}
			}
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x002017E8 File Offset: 0x001FF9E8
		public RitualOutcomeComp_Data DataForComp(RitualOutcomeComp comp)
		{
			int num = this.def.comps.IndexOf(comp);
			if (num != -1)
			{
				return this.compDatas[num];
			}
			Log.Error("Can't find index for " + comp.GetType().Name);
			return null;
		}

		// Token: 0x06005DA0 RID: 23968
		public abstract void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual);

		// Token: 0x06005DA1 RID: 23969 RVA: 0x00201834 File Offset: 0x001FFA34
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualOutcomeEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualOutcomeComp_Data>(ref this.compDatas, "compDatas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (this.compDatas.NullOrEmpty<RitualOutcomeComp_Data>() || this.compDatas.Count != this.def.comps.Count))
			{
				this.FillCompData();
			}
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x0020189F File Offset: 0x001FFA9F
		public Thought_Memory MakeMemory(Pawn p, LordJob_Ritual ritual, ThoughtDef overrideDef = null)
		{
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(overrideDef ?? this.def.memoryDef);
			thought_Memory.sourcePrecept = ritual.Ritual;
			return thought_Memory;
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x002018C8 File Offset: 0x001FFAC8
		public virtual void Tick(LordJob_Ritual ritual, float progressAmount = 1f)
		{
			if (!this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					ritualOutcomeComp.Tick(ritual, this.DataForComp(ritualOutcomeComp), progressAmount);
				}
			}
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			return null;
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExpectedQualityLabel()
		{
			return null;
		}

		// Token: 0x04003629 RID: 13865
		public RitualOutcomeEffectDef def;

		// Token: 0x0400362A RID: 13866
		public List<RitualOutcomeComp_Data> compDatas;
	}
}
