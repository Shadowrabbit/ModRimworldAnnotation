using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115B RID: 4443
	public class FocusStrengthOffset_BuildingDefs : FocusStrengthOffset
	{
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x0023D968 File Offset: 0x0023BB68
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

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x0023D9C8 File Offset: 0x0023BBC8
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

		// Token: 0x06006AC3 RID: 27331 RVA: 0x0023DA28 File Offset: 0x0023BC28
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

		// Token: 0x06006AC4 RID: 27332 RVA: 0x0023DA98 File Offset: 0x0023BC98
		protected virtual int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			return Math.Min(parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent).Count, this.maxBuildings);
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x0023DAE5 File Offset: 0x0023BCE5
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned && this.BuildingCount(parent) > 0;
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x0023DAFB File Offset: 0x0023BCFB
		protected virtual float OffsetForBuilding(Thing b)
		{
			return this.OffsetFor(b.def);
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x0023DB0C File Offset: 0x0023BD0C
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			int num = 0;
			while (num < forCell.Count && num < this.maxBuildings)
			{
				GenDraw.DrawLineBetween(parent.TrueCenter(), forCell[num].TrueCenter(), SimpleColor.Green, 0.2f);
				num++;
			}
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x0023DB94 File Offset: 0x0023BD94
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x0023DC30 File Offset: 0x0023BE30
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x0023DCAB File Offset: 0x0023BEAB
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.MaxOffsetPerBuilding;
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x0023DCBC File Offset: 0x0023BEBC
		protected float OffsetFor(ThingDef def)
		{
			MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == def);
			if (meditationFocusOffsetPerBuilding != null)
			{
				return meditationFocusOffsetPerBuilding.offset;
			}
			return 0f;
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x0023DD00 File Offset: 0x0023BF00
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

		// Token: 0x04003B5C RID: 15196
		public List<MeditationFocusOffsetPerBuilding> defs = new List<MeditationFocusOffsetPerBuilding>();

		// Token: 0x04003B5D RID: 15197
		public float radius = 10f;

		// Token: 0x04003B5E RID: 15198
		public int maxBuildings = int.MaxValue;

		// Token: 0x04003B5F RID: 15199
		public float offsetPerBuilding;

		// Token: 0x04003B60 RID: 15200
		[NoTranslate]
		public string explanationKey;

		// Token: 0x04003B61 RID: 15201
		[NoTranslate]
		public string explanationKeyAbstract;

		// Token: 0x04003B62 RID: 15202
		protected float minOffsetPerBuilding = float.MaxValue;

		// Token: 0x04003B63 RID: 15203
		protected float maxOffsetPerBuilding = float.MinValue;
	}
}
