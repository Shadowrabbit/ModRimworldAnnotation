using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EED RID: 3821
	public class Precept_Building : Precept_ThingStyle
	{
		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x001F5DB8 File Offset: 0x001F3FB8
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoBuildingName";
			}
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x001F5DBF File Offset: 0x001F3FBF
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology building"))
			{
				return;
			}
			this.GeneratePresenceDemand();
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x001F5DDC File Offset: 0x001F3FDC
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.GeneratePresenceDemand();
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.ClearTipCache();
			this.Notify_ThingDefSet();
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x001F5E00 File Offset: 0x001F4000
		private void GeneratePresenceDemand()
		{
			this.presenceDemand = new IdeoBuildingPresenceDemand(this);
			this.presenceDemand.minExpectation = this.def.buildingMinExpectations.RandomElement<ExpectationDef>();
			int num = Mathf.CeilToInt(this.def.roomRequirementCountCurve.Evaluate(Rand.Value));
			if ((base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable) && num > 0)
			{
				this.presenceDemand.roomRequirements = new List<RoomRequirement>();
				List<RoomRequirement> list = this.def.buildingRoomRequirements.ToList<RoomRequirement>();
				list.Shuffle<RoomRequirement>();
				for (int i = 0; i < num; i++)
				{
					this.presenceDemand.roomRequirements.Add(list[i]);
				}
				this.presenceDemand.roomRequirements.AddRange(this.def.buildingRoomRequirementsFixed);
			}
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001F5ED7 File Offset: 0x001F40D7
		public override string TransformThingLabel(string label)
		{
			return label + " (" + base.LabelCap + ")";
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x001F5EF0 File Offset: 0x001F40F0
		private string CostListString()
		{
			List<string> list = new List<string>();
			if (!base.ThingDef.CostList.NullOrEmpty<ThingDefCountClass>())
			{
				for (int i = 0; i < base.ThingDef.CostList.Count; i++)
				{
					list.Add(base.ThingDef.CostList[i].thingDef.LabelCap + " x" + base.ThingDef.CostList[i].count);
				}
			}
			if (!base.ThingDef.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				string text = "";
				for (int j = 0; j < base.ThingDef.stuffCategories.Count; j++)
				{
					if (j > 0)
					{
						text += "/";
					}
					text += base.ThingDef.stuffCategories[j].label;
				}
				list.Add(text.CapitalizeFirst() + " x" + base.ThingDef.CostStuffCount);
			}
			return list.ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x001F6014 File Offset: 0x001F4214
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				ThingDef thingDef = base.ThingDef;
				if (((thingDef != null) ? thingDef.description : null) != null)
				{
					Precept.tmpCompsDesc.Append(base.ThingDef.description);
				}
				else if (!base.Description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(base.Description);
				}
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingVariationOf".Translate() + ": "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ThingDef.LabelCap.Resolve().Indented("    "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Size".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(base.ThingDef.size.x + "x" + base.ThingDef.size.z);
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Cost".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(this.CostListString());
				if (base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingMinimumExpectations".Translate() + ": "));
					Precept.tmpCompsDesc.Append(this.presenceDemand.minExpectation.LabelCap);
					if (!this.presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoomRequirements".Translate() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(this.presenceDemand.RoomRequirementsInfo.ToLineList(" -  ", true));
					}
				}
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x001F6280 File Offset: 0x001F4480
		public override IEnumerable<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			if (!pawn.IsFreeColonist)
			{
				yield break;
			}
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null)
			{
				if (!this.presenceDemand.AppliesTo(mapHeld))
				{
					yield break;
				}
				if (!this.presenceDemand.BuildingPresent(mapHeld))
				{
					if (!activeThoughts.Any(delegate(Thought_Situational t)
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding2;
						return (thought_IdeoMissingBuilding2 = (t as Thought_IdeoMissingBuilding)) != null && thought_IdeoMissingBuilding2.demand == this.presenceDemand;
					}))
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding = (Thought_IdeoMissingBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingMissing);
						if (thought_IdeoMissingBuilding != null)
						{
							thought_IdeoMissingBuilding.pawn = pawn;
							thought_IdeoMissingBuilding.demand = this.presenceDemand;
							thought_IdeoMissingBuilding.sourcePrecept = this;
							yield return thought_IdeoMissingBuilding;
						}
					}
				}
				else if (!this.presenceDemand.RequirementsSatisfied(mapHeld) && !activeThoughts.Any(delegate(Thought_Situational t)
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding2;
					return (thought_IdeoDisrespectedBuilding2 = (t as Thought_IdeoDisrespectedBuilding)) != null && thought_IdeoDisrespectedBuilding2.demand == this.presenceDemand;
				}))
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding = (Thought_IdeoDisrespectedBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingDisrespected);
					if (thought_IdeoDisrespectedBuilding != null)
					{
						thought_IdeoDisrespectedBuilding.pawn = pawn;
						thought_IdeoDisrespectedBuilding.demand = this.presenceDemand;
						thought_IdeoDisrespectedBuilding.sourcePrecept = this;
						yield return thought_IdeoDisrespectedBuilding;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x001F629E File Offset: 0x001F449E
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x001F62AE File Offset: 0x001F44AE
		public override IEnumerable<Alert> GetAlerts()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || !this.presenceDemand.AppliesTo(currentMap))
			{
				yield break;
			}
			if (!this.presenceDemand.BuildingPresent(currentMap))
			{
				yield return this.presenceDemand.AlertCachedMissingMissing;
			}
			else if (!this.presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				yield return this.presenceDemand.AlertCachedMissingDisrespected;
			}
			yield break;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x001F62BE File Offset: 0x001F44BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoBuildingPresenceDemand>(ref this.presenceDemand, "presenceDemand", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.presenceDemand.parent = this;
			}
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x001F62F0 File Offset: 0x001F44F0
		public override string InspectStringExtra(Thing thing)
		{
			return ("Stat_Thing_RelatedToIdeos_Name".Translate() + ": " + this.ideo.name.ApplyTag(this.ideo)).Resolve();
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x001F6334 File Offset: 0x001F4534
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), this.ideo.name.ApplyTag(this.ideo).Resolve(), "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(this.ideo)
			}, false);
			yield break;
		}

		// Token: 0x04003519 RID: 13593
		public IdeoBuildingPresenceDemand presenceDemand;
	}
}
