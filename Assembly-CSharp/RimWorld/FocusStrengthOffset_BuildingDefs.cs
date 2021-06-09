using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F0 RID: 6128
	public class FocusStrengthOffset_BuildingDefs : FocusStrengthOffset
	{
		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x060087A1 RID: 34721 RVA: 0x0027BFE4 File Offset: 0x0027A1E4
		public float MaxOffsetPerBuilding
		{
			get
			{
				if (this.maxOffsetPerBuilding == -3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset > this.maxOffsetPerBuilding)
						{
							this.maxOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.maxOffsetPerBuilding;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x060087A2 RID: 34722 RVA: 0x0027C044 File Offset: 0x0027A244
		public float MinOffsetPerBuilding
		{
			get
			{
				if (this.minOffsetPerBuilding == 3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset < this.minOffsetPerBuilding)
						{
							this.minOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.minOffsetPerBuilding;
			}
		}

		// Token: 0x060087A3 RID: 34723 RVA: 0x0027C0A4 File Offset: 0x0027A2A4
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				float num = 0f;
				List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
				int num2 = 0;
				while (num2 < forCell.Count && num2 < this.maxBuildings)
				{
					num += this.OffsetForBuilding(forCell[num2]);
					num2++;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x060087A4 RID: 34724 RVA: 0x0027C114 File Offset: 0x0027A314
		protected virtual int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			return Math.Min(parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent).Count, this.maxBuildings);
		}

		// Token: 0x060087A5 RID: 34725 RVA: 0x0005B046 File Offset: 0x00059246
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned && this.BuildingCount(parent) > 0;
		}

		// Token: 0x060087A6 RID: 34726 RVA: 0x0005B05C File Offset: 0x0005925C
		protected virtual float OffsetForBuilding(Thing b)
		{
			return this.OffsetFor(b.def);
		}

		// Token: 0x060087A7 RID: 34727 RVA: 0x0027C164 File Offset: 0x0027A364
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			int num = 0;
			while (num < forCell.Count && num < this.maxBuildings)
			{
				GenDraw.DrawLineBetween(parent.TrueCenter(), forCell[num].TrueCenter(), SimpleColor.Green);
				num++;
			}
		}

		// Token: 0x060087A8 RID: 34728 RVA: 0x0027C1E8 File Offset: 0x0027A3E8
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060087A9 RID: 34729 RVA: 0x0027C284 File Offset: 0x0027A484
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x060087AA RID: 34730 RVA: 0x0005B06A File Offset: 0x0005926A
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.MaxOffsetPerBuilding;
		}

		// Token: 0x060087AB RID: 34731 RVA: 0x0027C300 File Offset: 0x0027A500
		protected float OffsetFor(ThingDef def)
		{
			MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == def);
			if (meditationFocusOffsetPerBuilding != null)
			{
				return meditationFocusOffsetPerBuilding.offset;
			}
			return 0f;
		}

		// Token: 0x060087AC RID: 34732 RVA: 0x0027C344 File Offset: 0x0027A544
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding in this.defs)
			{
				if (meditationFocusOffsetPerBuilding.offset == -3.4028235E+38f)
				{
					meditationFocusOffsetPerBuilding.offset = this.offsetPerBuilding;
				}
			}
		}

		// Token: 0x04005704 RID: 22276
		public List<MeditationFocusOffsetPerBuilding> defs = new List<MeditationFocusOffsetPerBuilding>();

		// Token: 0x04005705 RID: 22277
		public float radius = 10f;

		// Token: 0x04005706 RID: 22278
		public int maxBuildings = int.MaxValue;

		// Token: 0x04005707 RID: 22279
		public float offsetPerBuilding;

		// Token: 0x04005708 RID: 22280
		[NoTranslate]
		public string explanationKey;

		// Token: 0x04005709 RID: 22281
		[NoTranslate]
		public string explanationKeyAbstract;

		// Token: 0x0400570A RID: 22282
		protected float minOffsetPerBuilding = float.MaxValue;

		// Token: 0x0400570B RID: 22283
		protected float maxOffsetPerBuilding = float.MinValue;
	}
}
