using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A74 RID: 6772
	public abstract class Transferable : IExposable
	{
		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x0600956F RID: 38255
		public abstract Thing AnyThing { get; }

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06009570 RID: 38256
		public abstract ThingDef ThingDef { get; }

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x06009571 RID: 38257
		public abstract bool Interactive { get; }

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06009572 RID: 38258
		public abstract bool HasAnyThing { get; }

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06009573 RID: 38259 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x06009574 RID: 38260
		public abstract string Label { get; }

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06009575 RID: 38261 RVA: 0x00063CD5 File Offset: 0x00061ED5
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06009576 RID: 38262
		public abstract string TipDescription { get; }

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06009577 RID: 38263
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x06009578 RID: 38264
		// (set) Token: 0x06009579 RID: 38265
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x0600957A RID: 38266 RVA: 0x00063CE8 File Offset: 0x00061EE8
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

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x0600957B RID: 38267 RVA: 0x00063D00 File Offset: 0x00061F00
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

		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x0600957C RID: 38268 RVA: 0x00063D18 File Offset: 0x00061F18
		// (set) Token: 0x0600957D RID: 38269 RVA: 0x00063D20 File Offset: 0x00061F20
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

		// Token: 0x0600957E RID: 38270
		public abstract int GetMinimumToTransfer();

		// Token: 0x0600957F RID: 38271
		public abstract int GetMaximumToTransfer();

		// Token: 0x06009580 RID: 38272 RVA: 0x00063D29 File Offset: 0x00061F29
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x06009581 RID: 38273 RVA: 0x00063D38 File Offset: 0x00061F38
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x06009582 RID: 38274 RVA: 0x00063D4C File Offset: 0x00061F4C
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06009583 RID: 38275 RVA: 0x00063D5C File Offset: 0x00061F5C
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

		// Token: 0x06009584 RID: 38276 RVA: 0x00063D98 File Offset: 0x00061F98
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06009585 RID: 38277 RVA: 0x002B5EDC File Offset: 0x002B40DC
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts", false);
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		// Token: 0x06009586 RID: 38278 RVA: 0x00063DA8 File Offset: 0x00061FA8
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06009587 RID: 38279 RVA: 0x00063DB1 File Offset: 0x00061FB1
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		// Token: 0x06009588 RID: 38280 RVA: 0x00063DCB File Offset: 0x00061FCB
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		// Token: 0x06009589 RID: 38281 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		// Token: 0x0600958A RID: 38282 RVA: 0x00063DE5 File Offset: 0x00061FE5
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x0600958B RID: 38283 RVA: 0x00063DE5 File Offset: 0x00061FE5
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0600958C RID: 38284 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x04005F1B RID: 24347
		private string editBuffer = "0";
	}
}
