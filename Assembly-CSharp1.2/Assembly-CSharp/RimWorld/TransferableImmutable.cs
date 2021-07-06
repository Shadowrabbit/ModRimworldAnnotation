using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A75 RID: 6773
	public class TransferableImmutable : Transferable
	{
		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x0600958E RID: 38286 RVA: 0x00063E00 File Offset: 0x00062000
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x0600958F RID: 38287 RVA: 0x0005F3CB File Offset: 0x0005D5CB
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x06009590 RID: 38288 RVA: 0x00063E18 File Offset: 0x00062018
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06009591 RID: 38289 RVA: 0x0005F36B File Offset: 0x0005D56B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06009592 RID: 38290 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06009593 RID: 38291 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06009594 RID: 38292 RVA: 0x0005F3F9 File Offset: 0x0005D5F9
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06009595 RID: 38293 RVA: 0x0000A2E4 File Offset: 0x000084E4
		// (set) Token: 0x06009596 RID: 38294 RVA: 0x00063E28 File Offset: 0x00062028
		public override int CountToTransfer
		{
			get
			{
				return 0;
			}
			protected set
			{
				if (value != 0)
				{
					throw new InvalidOperationException("immutable transferable");
				}
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x06009597 RID: 38295 RVA: 0x002B5F14 File Offset: 0x002B4114
		public string LabelWithTotalStackCount
		{
			get
			{
				string text = this.Label;
				int totalStackCount = this.TotalStackCount;
				if (totalStackCount != 1)
				{
					text = text + " x" + totalStackCount.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x06009598 RID: 38296 RVA: 0x00063E38 File Offset: 0x00062038
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x06009599 RID: 38297 RVA: 0x002B5F48 File Offset: 0x002B4148
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0600959A RID: 38298 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x0600959B RID: 38299 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x0600959C RID: 38300 RVA: 0x00063DE5 File Offset: 0x00061FE5
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0600959D RID: 38301 RVA: 0x002B5F84 File Offset: 0x002B4184
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}

		// Token: 0x04005F1C RID: 24348
		public List<Thing> things = new List<Thing>();
	}
}
