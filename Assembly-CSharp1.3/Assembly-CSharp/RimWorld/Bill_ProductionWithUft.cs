using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001089 RID: 4233
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060064C7 RID: 25799 RVA: 0x0021F72C File Offset: 0x0021D92C
		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					return (base.StatusString ?? "").Trim();
				}
				return ("BoundWorkerIs".Translate(this.BoundWorker.LabelShort, this.BoundWorker) + base.StatusString).Trim();
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0021F794 File Offset: 0x0021D994
		public Pawn BoundWorker
		{
			get
			{
				if (this.boundUftInt == null)
				{
					return null;
				}
				Pawn creator = this.boundUftInt.Creator;
				if (creator == null || creator.Downed || creator.HostFaction != null || creator.Destroyed || !creator.Spawned)
				{
					this.boundUftInt = null;
					return null;
				}
				Thing thing = this.billStack.billGiver as Thing;
				if (thing != null)
				{
					WorkTypeDef workTypeDef = null;
					List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].fixedBillGiverDefs != null && allDefsListForReading[i].fixedBillGiverDefs.Contains(thing.def))
						{
							workTypeDef = allDefsListForReading[i].workType;
							break;
						}
					}
					if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
					{
						this.boundUftInt = null;
						return null;
					}
				}
				return creator;
			}
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x060064C9 RID: 25801 RVA: 0x0021F868 File Offset: 0x0021DA68
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x0021F870 File Offset: 0x0021DA70
		public void SetBoundUft(UnfinishedThing value, bool setOtherLink = true)
		{
			if (value == this.boundUftInt)
			{
				return;
			}
			UnfinishedThing unfinishedThing = this.boundUftInt;
			this.boundUftInt = value;
			if (setOtherLink)
			{
				if (unfinishedThing != null && unfinishedThing.BoundBill == this)
				{
					unfinishedThing.BoundBill = null;
				}
				if (value != null && value.BoundBill != this)
				{
					this.boundUftInt.BoundBill = this;
				}
			}
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x0021F8C3 File Offset: 0x0021DAC3
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x0021F8CB File Offset: 0x0021DACB
		public Bill_ProductionWithUft(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x0021F8D5 File Offset: 0x0021DAD5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x0021F8EE File Offset: 0x0021DAEE
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x0021F8FE File Offset: 0x0021DAFE
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x0021F907 File Offset: 0x0021DB07
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}

		// Token: 0x040038B9 RID: 14521
		private UnfinishedThing boundUftInt;
	}
}
