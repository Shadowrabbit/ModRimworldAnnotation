using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001312 RID: 4882
	[StaticConstructorOnStartup]
	public static class TrainingCardUtility
	{
		// Token: 0x06007598 RID: 30104 RVA: 0x00288774 File Offset: 0x00286974
		public static void DrawTrainingCard(Rect rect, Pawn pawn)
		{
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(TrainingCardUtility.TrainabilityLeft, TrainingCardUtility.TrainabilityTop, 30f, 30f);
			TooltipHandler.TipRegionByKey(rect2, "RenameAnimal");
			if (Widgets.ButtonImage(rect2, TexButton.Rename, true))
			{
				Find.WindowStack.Add(new Dialog_NamePawn(pawn));
			}
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect);
			listing_Standard.Label("CreatureTrainability".Translate(pawn.def.label).CapitalizeFirst() + ": " + pawn.RaceProps.trainability.LabelCap, 22f, null);
			listing_Standard.Label("CreatureWildness".Translate(pawn.def.label).CapitalizeFirst() + ": " + pawn.RaceProps.wildness.ToStringPercent(), 22f, TrainableUtility.GetWildnessExplanation(pawn.def));
			if (pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				Rect rect3 = listing_Standard.GetRect(25f);
				Widgets.Label(rect3, "Master".Translate() + ": ");
				rect3.xMin = rect3.center.x;
				if (pawn.RaceProps.playerCanChangeMaster || !ModsConfig.IdeologyActive)
				{
					TrainableUtility.MasterSelectButton(rect3, pawn, false);
				}
				else
				{
					Pawn_PlayerSettings playerSettings = pawn.playerSettings;
					if (((playerSettings != null) ? playerSettings.Master : null) != null)
					{
						Widgets.Label(rect3, TrainableUtility.MasterString(pawn).Truncate(rect3.width, null));
						TooltipHandler.TipRegion(rect3, "DryadCannotChangeMaster".Translate(pawn.Named("ANIMAL"), pawn.playerSettings.Master.Named("MASTER")).CapitalizeFirst());
					}
				}
				listing_Standard.Gap(12f);
				Rect rect4 = listing_Standard.GetRect(25f);
				bool followDrafted = pawn.playerSettings.followDrafted;
				Widgets.CheckboxLabeled(rect4, "CreatureFollowDrafted".Translate(), ref followDrafted, false, null, null, false);
				if (followDrafted != pawn.playerSettings.followDrafted)
				{
					pawn.playerSettings.followDrafted = followDrafted;
				}
				Rect rect5 = listing_Standard.GetRect(25f);
				bool followFieldwork = pawn.playerSettings.followFieldwork;
				Widgets.CheckboxLabeled(rect5, "CreatureFollowFieldwork".Translate(), ref followFieldwork, false, null, null, false);
				if (followFieldwork != pawn.playerSettings.followFieldwork)
				{
					pawn.playerSettings.followFieldwork = followFieldwork;
				}
			}
			if (pawn.RaceProps.showTrainables)
			{
				listing_Standard.Gap(12f);
				float num = 50f;
				List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
				for (int i = 0; i < trainableDefsInListOrder.Count; i++)
				{
					if (TrainingCardUtility.TryDrawTrainableRow(listing_Standard.GetRect(28f), pawn, trainableDefsInListOrder[i]))
					{
						num += 28f;
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06007599 RID: 30105 RVA: 0x00288A60 File Offset: 0x00286C60
		public static float TotalHeightForPawn(Pawn p)
		{
			if (p == null)
			{
				return 0f;
			}
			int num = 0;
			if (p.RaceProps.showTrainables)
			{
				for (int i = 0; i < DefDatabase<TrainableDef>.AllDefsListForReading.Count; i++)
				{
					bool flag;
					p.training.CanAssignToTrain(DefDatabase<TrainableDef>.AllDefsListForReading[i], out flag);
					if (flag)
					{
						num++;
					}
				}
			}
			float num2 = 112f + 28f * (float)num;
			if (p.training.HasLearned(TrainableDefOf.Obedience))
			{
				num2 += 75f;
				num2 += 12f;
			}
			return num2;
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x00288AEC File Offset: 0x00286CEC
		private static bool TryDrawTrainableRow(Rect rect, Pawn pawn, TrainableDef td)
		{
			bool flag = pawn.training.HasLearned(td);
			bool flag2;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(td, out flag2);
			if (!flag2)
			{
				return false;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = rect;
			rect2.width -= 50f;
			rect2.xMin += (float)td.indent * 10f;
			Rect rect3 = rect;
			rect3.xMin = rect3.xMax - 50f + 17f;
			TrainingCardUtility.DoTrainableCheckbox(rect2, pawn, td, canTrain, true, false);
			if (flag)
			{
				GUI.color = Color.green;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, pawn.training.GetSteps(td) + " / " + td.steps);
			Text.Anchor = TextAnchor.UpperLeft;
			if (DebugSettings.godMode && !pawn.training.HasLearned(td))
			{
				Rect rect4 = rect3;
				rect4.yMin = rect4.yMax - 10f;
				rect4.xMin = rect4.xMax - 10f;
				if (Widgets.ButtonText(rect4, "+", true, true, true))
				{
					pawn.training.Train(td, pawn.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>(), false);
				}
			}
			TrainingCardUtility.DoTrainableTooltip(rect, pawn, td, canTrain);
			GUI.color = Color.white;
			return true;
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x00288C48 File Offset: 0x00286E48
		public static void DoTrainableCheckbox(Rect rect, Pawn pawn, TrainableDef td, AcceptanceReport canTrain, bool drawLabel, bool doTooltip)
		{
			bool flag = pawn.training.HasLearned(td);
			bool wanted = pawn.training.GetWanted(td);
			bool flag2 = wanted;
			Texture2D texChecked = flag ? TrainingCardUtility.LearnedTrainingTex : null;
			Texture2D texUnchecked = flag ? TrainingCardUtility.LearnedNotTrainingTex : null;
			if (drawLabel)
			{
				Widgets.CheckboxLabeled(rect, td.LabelCap, ref wanted, !canTrain.Accepted, texChecked, texUnchecked, false);
			}
			else
			{
				Widgets.Checkbox(rect.position, ref wanted, rect.width, !canTrain.Accepted, true, texChecked, texUnchecked);
			}
			if (wanted != flag2)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTraining, KnowledgeAmount.Total);
				pawn.training.SetWantedRecursive(td, wanted);
			}
			if (doTooltip)
			{
				TrainingCardUtility.DoTrainableTooltip(rect, pawn, td, canTrain);
			}
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x00288CF8 File Offset: 0x00286EF8
		private static void DoTrainableTooltip(Rect rect, Pawn pawn, TrainableDef td, AcceptanceReport canTrain)
		{
			if (!Mouse.IsOver(rect))
			{
				return;
			}
			TooltipHandler.TipRegion(rect, delegate()
			{
				string text = td.LabelCap + "\n\n" + td.description;
				if (!canTrain.Accepted)
				{
					text = text + "\n\n" + canTrain.Reason;
				}
				else if (!td.prerequisites.NullOrEmpty<TrainableDef>())
				{
					text += "\n";
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						if (!pawn.training.HasLearned(td.prerequisites[i]))
						{
							text += "\n" + "TrainingNeedsPrerequisite".Translate(td.prerequisites[i].LabelCap);
						}
					}
				}
				return text;
			}, (int)(rect.y * 612f + rect.x));
		}

		// Token: 0x04004119 RID: 16665
		public const float RowHeight = 28f;

		// Token: 0x0400411A RID: 16666
		private const float InfoHeaderHeight = 50f;

		// Token: 0x0400411B RID: 16667
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityLeft = 220f;

		// Token: 0x0400411C RID: 16668
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityTop = 0f;

		// Token: 0x0400411D RID: 16669
		private static readonly Texture2D LearnedTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheck", true);

		// Token: 0x0400411E RID: 16670
		private static readonly Texture2D LearnedNotTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheckOff", true);
	}
}
