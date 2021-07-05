using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001316 RID: 4886
	public class TransferableOneWay : Transferable
	{
		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x060075CE RID: 30158 RVA: 0x00289055 File Offset: 0x00287255
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

		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x060075CF RID: 30159 RVA: 0x002541FB File Offset: 0x002523FB
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

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x060075D0 RID: 30160 RVA: 0x0028906D File Offset: 0x0028726D
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x060075D1 RID: 30161 RVA: 0x0025415B File Offset: 0x0025235B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x060075D2 RID: 30162 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x060075D3 RID: 30163 RVA: 0x000126F5 File Offset: 0x000108F5
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x060075D4 RID: 30164 RVA: 0x00254229 File Offset: 0x00252429
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

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x060075D5 RID: 30165 RVA: 0x0028907D File Offset: 0x0028727D
		// (set) Token: 0x060075D6 RID: 30166 RVA: 0x00289085 File Offset: 0x00287285
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x060075D7 RID: 30167 RVA: 0x0028909C File Offset: 0x0028729C
		public int MaxCount
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

		// Token: 0x060075D8 RID: 30168 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x002890D6 File Offset: 0x002872D6
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x060075DA RID: 30170 RVA: 0x002890DE File Offset: 0x002872DE
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x002890F4 File Offset: 0x002872F4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x04004124 RID: 16676
		public List<Thing> things = new List<Thing>();

		// Token: 0x04004125 RID: 16677
		private int countToTransfer;
	}
}
