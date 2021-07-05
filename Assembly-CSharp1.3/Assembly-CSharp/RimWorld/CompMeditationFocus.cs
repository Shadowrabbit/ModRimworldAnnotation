using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001150 RID: 4432
	public class CompMeditationFocus : CompStatOffsetBase
	{
		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06006A7E RID: 27262 RVA: 0x0023D04F File Offset: 0x0023B24F
		public new CompProperties_MeditationFocus Props
		{
			get
			{
				return (CompProperties_MeditationFocus)this.props;
			}
		}

		// Token: 0x06006A7F RID: 27263 RVA: 0x0023D05C File Offset: 0x0023B25C
		public override float GetStatOffset(Pawn pawn = null)
		{
			if (!ModLister.CheckRoyalty("Meditation focus"))
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				if (this.Props.offsets[i].CanApply(this.parent, pawn))
				{
					num += this.Props.offsets[i].GetOffset(this.parent, pawn);
				}
			}
			return num;
		}

		// Token: 0x06006A80 RID: 27264 RVA: 0x0023D0DC File Offset: 0x0023B2DC
		public override IEnumerable<string> GetExplanation()
		{
			int num;
			for (int i = 0; i < this.Props.offsets.Count; i = num + 1)
			{
				string explanation = this.Props.offsets[i].GetExplanation(this.parent);
				if (!explanation.NullOrEmpty())
				{
					yield return explanation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006A81 RID: 27265 RVA: 0x0023D0EC File Offset: 0x0023B2EC
		public bool CanPawnUse(Pawn pawn)
		{
			for (int i = 0; i < this.Props.focusTypes.Count; i++)
			{
				if (this.Props.focusTypes[i].CanPawnUse(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x0023D130 File Offset: 0x0023B330
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			using (List<FocusStrengthOffset>.Enumerator enumerator = this.Props.offsets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings;
					if ((focusStrengthOffset_ArtificialBuildings = (enumerator.Current as FocusStrengthOffset_ArtificialBuildings)) != null && MeditationUtility.CountsAsArtificialBuilding(def, faction) && cellRect.ClosestCellTo(this.parent.Position).DistanceTo(this.parent.Position) <= focusStrengthOffset_ArtificialBuildings.radius)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x0023D1D4 File Offset: 0x0023B3D4
		public override string CompInspectStringExtra()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (base.LastUser != null)
			{
				stringBuilder.Append("UserMeditationFocusStrength".Translate(base.LastUser.Named("LASTUSER")) + ": " + this.Props.statDef.ValueToString(this.parent.GetStatValueForPawn(this.Props.statDef, base.LastUser, true), ToStringNumberSense.Absolute, true));
			}
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				string text = this.Props.offsets[i].InspectStringExtra(this.parent, null);
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0023D2BB File Offset: 0x0023B4BB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Meditation, "MeditationFocuses".Translate(), (from f in this.Props.focusTypes
				select f.label).ToCommaList(false, false).CapitalizeFirst(), "MeditationFocusesDesc".Translate(), 99995, null, null, false);
			}
			yield break;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x0023D2CC File Offset: 0x0023B4CC
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.Props.offsets.Count; i++)
				{
					this.Props.offsets[i].PostDrawExtraSelectionOverlays(this.parent, base.LastUser);
				}
			}
		}
	}
}
