using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A71 RID: 6769
	[StaticConstructorOnStartup]
	public static class TrainingCardUtility
	{
		// Token: 0x06009567 RID: 38247 RVA: 0x002B589C File Offset: 0x002B3A9C
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
				TrainableUtility.MasterSelectButton(rect3, pawn, false);
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
			listing_Standard.End();
		}

		// Token: 0x06009568 RID: 38248 RVA: 0x002B5AF8 File Offset: 0x002B3CF8
		public static float TotalHeightForPawn(Pawn p)
		{
			if (p == null)
			{
				return 0f;
			}
			int num = 0;
			for (int i = 0; i < DefDatabase<TrainableDef>.AllDefsListForReading.Count; i++)
			{
				bool flag;
				p.training.CanAssignToTrain(DefDatabase<TrainableDef>.AllDefsListForReading[i], out flag);
				if (flag)
				{
					num++;
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

		// Token: 0x06009569 RID: 38249 RVA: 0x002B5B78 File Offset: 0x002B3D78
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

		// Token: 0x0600956A RID: 38250 RVA: 0x002B5CD4 File Offset: 0x002B3ED4
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

		// Token: 0x0600956B RID: 38251 RVA: 0x002B5D84 File Offset: 0x002B3F84
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

		// Token: 0x04005F0F RID: 24335
		public const float RowHeight = 28f;

		// Token: 0x04005F10 RID: 24336
		private const float InfoHeaderHeight = 50f;

		// Token: 0x04005F11 RID: 24337
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityLeft = 220f;

		// Token: 0x04005F12 RID: 24338
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityTop = 0f;

		// Token: 0x04005F13 RID: 24339
		private static readonly Texture2D LearnedTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheck", true);

		// Token: 0x04005F14 RID: 24340
		private static readonly Texture2D LearnedNotTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheckOff", true);
	}
}
