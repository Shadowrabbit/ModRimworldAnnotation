using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F7 RID: 1015
	[StaticConstructorOnStartup]
	public class Command_SetPlantToGrow : Command
	{
		// Token: 0x06001E8D RID: 7821 RVA: 0x000BEE6C File Offset: 0x000BD06C
		public Command_SetPlantToGrow()
		{
			this.tutorTag = "GrowingZoneSetPlant";
			ThingDef thingDef = null;
			bool flag = false;
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				IPlantToGrowSettable plantToGrowSettable = obj as IPlantToGrowSettable;
				if (plantToGrowSettable != null)
				{
					if (thingDef != null && thingDef != plantToGrowSettable.GetPlantDefToGrow())
					{
						flag = true;
						break;
					}
					thingDef = plantToGrowSettable.GetPlantDefToGrow();
				}
			}
			if (flag)
			{
				this.icon = Command_SetPlantToGrow.SetPlantToGrowTex;
				this.defaultLabel = "CommandSelectPlantToGrowMulti".Translate();
				return;
			}
			this.icon = thingDef.uiIcon;
			this.iconAngle = thingDef.uiIconAngle;
			this.iconOffset = thingDef.uiIconOffset;
			this.defaultLabel = "CommandSelectPlantToGrow".Translate(thingDef.LabelCap);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000BEF58 File Offset: 0x000BD158
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (this.settables == null)
			{
				this.settables = new List<IPlantToGrowSettable>();
			}
			if (!this.settables.Contains(this.settable))
			{
				this.settables.Add(this.settable);
			}
			Command_SetPlantToGrow.tmpAvailablePlants.Clear();
			foreach (ThingDef thingDef in PlantUtility.ValidPlantTypesForGrowers(this.settables))
			{
				if (Command_SetPlantToGrow.IsPlantAvailable(thingDef, this.settable.Map))
				{
					Command_SetPlantToGrow.tmpAvailablePlants.Add(thingDef);
				}
			}
			Command_SetPlantToGrow.tmpAvailablePlants.SortBy((ThingDef x) => -this.GetPlantListPriority(x), (ThingDef x) => x.label);
			for (int i = 0; i < Command_SetPlantToGrow.tmpAvailablePlants.Count; i++)
			{
				ThingDef plantDef = Command_SetPlantToGrow.tmpAvailablePlants[i];
				string text = plantDef.LabelCap;
				if (plantDef.plant.sowMinSkill > 0)
				{
					text = string.Concat(new object[]
					{
						text,
						" (" + "MinSkill".Translate() + ": ",
						plantDef.plant.sowMinSkill,
						")"
					});
				}
				list.Add(new FloatMenuOption(text, delegate()
				{
					string s = this.tutorTag + "-" + plantDef.defName;
					if (!TutorSystem.AllowAction(s))
					{
						return;
					}
					bool flag = true;
					for (int j = 0; j < this.settables.Count; j++)
					{
						this.settables[j].SetPlantDefToGrow(plantDef);
						if (flag && plantDef.plant.interferesWithRoof)
						{
							using (IEnumerator<IntVec3> enumerator2 = this.settables[j].Cells.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.Roofed(this.settables[j].Map))
									{
										Messages.Message("MessagePlantIncompatibleWithRoof".Translate(Find.ActiveLanguageWorker.Pluralize(plantDef.LabelCap, -1)), MessageTypeDefOf.CautionInput, false);
										flag = false;
										break;
									}
								}
							}
						}
					}
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant, KnowledgeAmount.Total);
					this.WarnAsAppropriate(plantDef);
					TutorSystem.Notify_Event(s);
				}, plantDef, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, plantDef), null, true, 0));
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000BF154 File Offset: 0x000BD354
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.settables == null)
			{
				this.settables = new List<IPlantToGrowSettable>();
			}
			this.settables.Add(((Command_SetPlantToGrow)other).settable);
			return false;
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000BF180 File Offset: 0x000BD380
		private void WarnAsAppropriate(ThingDef plantDef)
		{
			if (plantDef.plant.sowMinSkill > 0)
			{
				foreach (Pawn pawn in this.settable.Map.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.skills.GetSkill(SkillDefOf.Plants).Level >= plantDef.plant.sowMinSkill && !pawn.Downed && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Growing))
					{
						return;
					}
				}
				Find.WindowStack.Add(new Dialog_MessageBox("NoGrowerCanPlant".Translate(plantDef.label, plantDef.plant.sowMinSkill).CapitalizeFirst(), null, null, null, null, null, false, null, null));
			}
			if (plantDef.plant.cavePlant)
			{
				IntVec3 cell = IntVec3.Invalid;
				for (int i = 0; i < this.settables.Count; i++)
				{
					foreach (IntVec3 intVec in this.settables[i].Cells)
					{
						if (!intVec.Roofed(this.settables[i].Map) || this.settables[i].Map.glowGrid.GameGlowAt(intVec, true) > 0f)
						{
							cell = intVec;
							break;
						}
					}
					if (cell.IsValid)
					{
						break;
					}
				}
				if (cell.IsValid)
				{
					Messages.Message("MessageWarningCavePlantsExposedToLight".Translate(plantDef.LabelCap), new TargetInfo(cell, this.settable.Map, false), MessageTypeDefOf.RejectInput, true);
				}
			}
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000BF384 File Offset: 0x000BD584
		public static bool IsPlantAvailable(ThingDef plantDef, Map map)
		{
			List<ResearchProjectDef> sowResearchPrerequisites = plantDef.plant.sowResearchPrerequisites;
			if (sowResearchPrerequisites == null)
			{
				return true;
			}
			for (int i = 0; i < sowResearchPrerequisites.Count; i++)
			{
				if (!sowResearchPrerequisites[i].IsFinished)
				{
					return false;
				}
			}
			return !plantDef.plant.mustBeWildToSow || map.Biome.AllWildPlants.Contains(plantDef);
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000BF3E8 File Offset: 0x000BD5E8
		private float GetPlantListPriority(ThingDef plantDef)
		{
			if (plantDef.plant.IsTree)
			{
				return 1f;
			}
			switch (plantDef.plant.purpose)
			{
			case PlantPurpose.Food:
				return 4f;
			case PlantPurpose.Health:
				return 3f;
			case PlantPurpose.Beauty:
				return 2f;
			case PlantPurpose.Misc:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x04001299 RID: 4761
		public IPlantToGrowSettable settable;

		// Token: 0x0400129A RID: 4762
		private List<IPlantToGrowSettable> settables;

		// Token: 0x0400129B RID: 4763
		private static List<ThingDef> tmpAvailablePlants = new List<ThingDef>();

		// Token: 0x0400129C RID: 4764
		private static readonly Texture2D SetPlantToGrowTex = ContentFinder<Texture2D>.Get("UI/Commands/SetPlantToGrow", true);
	}
}
