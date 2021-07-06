using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A77 RID: 6775
	public class TransferableOneWay : Transferable
	{
		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x060095A3 RID: 38307 RVA: 0x00063E6A File Offset: 0x0006206A
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

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x060095A4 RID: 38308 RVA: 0x0005F3CB File Offset: 0x0005D5CB
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

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x060095A5 RID: 38309 RVA: 0x00063E82 File Offset: 0x00062082
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x060095A6 RID: 38310 RVA: 0x0005F36B File Offset: 0x0005D56B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x060095A7 RID: 38311 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x060095A8 RID: 38312 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x060095A9 RID: 38313 RVA: 0x0005F3F9 File Offset: 0x0005D5F9
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

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x060095AA RID: 38314 RVA: 0x00063E92 File Offset: 0x00062092
		// (set) Token: 0x060095AB RID: 38315 RVA: 0x00063E9A File Offset: 0x0006209A
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

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x060095AC RID: 38316 RVA: 0x002B6020 File Offset: 0x002B4220
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

		// Token: 0x060095AD RID: 38317 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x060095AE RID: 38318 RVA: 0x00063EAF File Offset: 0x000620AF
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x060095AF RID: 38319 RVA: 0x00063EB7 File Offset: 0x000620B7
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x060095B0 RID: 38320 RVA: 0x002B605C File Offset: 0x002B425C
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
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}

		// Token: 0x04005F20 RID: 24352
		public List<Thing> things = new List<Thing>();

		// Token: 0x04005F21 RID: 24353
		private int countToTransfer;
	}
}
