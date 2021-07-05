using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001315 RID: 4885
	public class TransferableImmutable : Transferable
	{
		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x060075BD RID: 30141 RVA: 0x00288EEA File Offset: 0x002870EA
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

		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x060075BE RID: 30142 RVA: 0x002541FB File Offset: 0x002523FB
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

		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x060075BF RID: 30143 RVA: 0x00288F02 File Offset: 0x00287102
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x060075C0 RID: 30144 RVA: 0x0025415B File Offset: 0x0025235B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x060075C1 RID: 30145 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x060075C2 RID: 30146 RVA: 0x000126F5 File Offset: 0x000108F5
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x060075C3 RID: 30147 RVA: 0x00254229 File Offset: 0x00252429
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

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x060075C4 RID: 30148 RVA: 0x0001276E File Offset: 0x0001096E
		// (set) Token: 0x060075C5 RID: 30149 RVA: 0x00288F12 File Offset: 0x00287112
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

		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x060075C6 RID: 30150 RVA: 0x00288F24 File Offset: 0x00287124
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

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x060075C7 RID: 30151 RVA: 0x00288F56 File Offset: 0x00287156
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x060075C8 RID: 30152 RVA: 0x00288F6C File Offset: 0x0028716C
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

		// Token: 0x060075C9 RID: 30153 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x00288ECF File Offset: 0x002870CF
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x00288FA8 File Offset: 0x002871A8
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
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x04004123 RID: 16675
		public List<Thing> things = new List<Thing>();
	}
}
