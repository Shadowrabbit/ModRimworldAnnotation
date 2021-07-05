using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001314 RID: 4884
	public abstract class Transferable : IExposable
	{
		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x0600759E RID: 30110
		public abstract Thing AnyThing { get; }

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x0600759F RID: 30111
		public abstract ThingDef ThingDef { get; }

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x060075A0 RID: 30112
		public abstract bool Interactive { get; }

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x060075A1 RID: 30113
		public abstract bool HasAnyThing { get; }

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x060075A2 RID: 30114 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x060075A3 RID: 30115
		public abstract string Label { get; }

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x060075A4 RID: 30116 RVA: 0x00288D87 File Offset: 0x00286F87
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x060075A5 RID: 30117
		public abstract string TipDescription { get; }

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x060075A6 RID: 30118
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x060075A7 RID: 30119
		// (set) Token: 0x060075A8 RID: 30120
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x060075A9 RID: 30121 RVA: 0x00288D9A File Offset: 0x00286F9A
		public int CountToTransferToSource
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return -this.CountToTransfer;
				}
				return this.CountToTransfer;
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x060075AA RID: 30122 RVA: 0x00288DB2 File Offset: 0x00286FB2
		public int CountToTransferToDestination
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return this.CountToTransfer;
				}
				return -this.CountToTransfer;
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x060075AB RID: 30123 RVA: 0x00288DCA File Offset: 0x00286FCA
		// (set) Token: 0x060075AC RID: 30124 RVA: 0x00288DD2 File Offset: 0x00286FD2
		public string EditBuffer
		{
			get
			{
				return this.editBuffer;
			}
			set
			{
				this.editBuffer = value;
			}
		}

		// Token: 0x060075AD RID: 30125
		public abstract int GetMinimumToTransfer();

		// Token: 0x060075AE RID: 30126
		public abstract int GetMaximumToTransfer();

		// Token: 0x060075AF RID: 30127 RVA: 0x00288DDB File Offset: 0x00286FDB
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x00288DEA File Offset: 0x00286FEA
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x00288DFE File Offset: 0x00286FFE
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x00288E0E File Offset: 0x0028700E
		public AcceptanceReport CanAdjustTo(int destination)
		{
			if (destination == this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (this.ClampAmount(destination) != this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (destination < this.CountToTransfer)
			{
				return this.UnderflowReport();
			}
			return this.OverflowReport();
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x00288E4A File Offset: 0x0028704A
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x00288E5C File Offset: 0x0028705C
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts");
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x00288E92 File Offset: 0x00287092
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x00288E9B File Offset: 0x0028709B
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x00288EB5 File Offset: 0x002870B5
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x00288ECF File Offset: 0x002870CF
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x00288ECF File Offset: 0x002870CF
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x04004122 RID: 16674
		private string editBuffer = "0";
	}
}
