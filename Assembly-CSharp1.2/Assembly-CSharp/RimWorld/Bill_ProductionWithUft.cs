using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D6 RID: 5846
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06008067 RID: 32871 RVA: 0x0026040C File Offset: 0x0025E60C
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

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06008068 RID: 32872 RVA: 0x00260474 File Offset: 0x0025E674
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

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06008069 RID: 32873 RVA: 0x000562E0 File Offset: 0x000544E0
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x00260548 File Offset: 0x0025E748
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

		// Token: 0x0600806B RID: 32875 RVA: 0x000562E8 File Offset: 0x000544E8
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x0600806C RID: 32876 RVA: 0x000562F0 File Offset: 0x000544F0
		public Bill_ProductionWithUft(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x000562F9 File Offset: 0x000544F9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x0600806E RID: 32878 RVA: 0x00056312 File Offset: 0x00054512
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x00056322 File Offset: 0x00054522
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x06008070 RID: 32880 RVA: 0x0005632B File Offset: 0x0005452B
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}

		// Token: 0x0400532A RID: 21290
		private UnfinishedThing boundUftInt;
	}
}
