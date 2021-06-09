using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001573 RID: 5491
	public static class TrainableUtility
	{
		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06007733 RID: 30515 RVA: 0x0005066F File Offset: 0x0004E86F
		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x00243D04 File Offset: 0x00241F04
		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			bool flag;
			do
			{
				flag = false;
				for (int i = 0; i < TrainableUtility.defsInListOrder.Count; i++)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[i];
					if (trainableDef.prerequisites != null)
					{
						for (int j = 0; j < trainableDef.prerequisites.Count; j++)
						{
							if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
							{
								trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			while (flag);
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x00050676 File Offset: 0x0004E876
		public static string MasterString(Pawn pawn)
		{
			if (pawn.playerSettings.Master == null)
			{
				return "(" + "NoneLower".TranslateSimple() + ")";
			}
			return RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x000506B0 File Offset: 0x0004E8B0
		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x00243DCC File Offset: 0x00241FCC
		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, Pawn>(rect, pawn, new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu), TrainableUtility.MasterString(pawn).Truncate(rect.width, null), null, TrainableUtility.MasterString(pawn), null, null, paintable);
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x000506C3 File Offset: 0x0004E8C3
		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x000506D0 File Offset: 0x0004E8D0
		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
				{
					p.playerSettings.Master = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn col = enumerator.Current;
					string text = RelationsUtility.LabelWithBondInfo(col, p);
					Action action = null;
					if (TrainableUtility.CanBeMaster(col, p, true))
					{
						action = delegate()
						{
							p.playerSettings.Master = col;
						};
					}
					else
					{
						int level = col.skills.GetSkill(SkillDefOf.Animals).Level;
						int num = TrainableUtility.MinimumHandlingSkill(p);
						if (level < num)
						{
							action = null;
							text += " (" + "SkillTooLow".Translate(SkillDefOf.Animals.LabelCap, level, num) + ")";
						}
					}
					yield return new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = col
					};
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x00243E18 File Offset: 0x00242018
		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				return false;
			}
			if (master.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				return true;
			}
			int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			return level >= num;
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x00243E74 File Offset: 0x00242074
		public static string GetIconTooltipText(Pawn pawn)
		{
			string text = "";
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
			}
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (allColonistBondsFor.Any<Pawn>())
			{
				text += string.Format("{0}: {1}\n", "BondedTo".Translate(), (from bond in allColonistBondsFor
				select bond.LabelShort).ToCommaList(true));
			}
			return text.TrimEndNewlines();
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x00243F2C File Offset: 0x0024212C
		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonist
			select bond.otherPawn;
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x000506E0 File Offset: 0x0004E8E0
		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.race.wildness) * 60000f);
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x00050702 File Offset: 0x0004E902
		public static bool TamenessCanDecay(ThingDef def)
		{
			return def.race.wildness > 0.101f;
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x00050716 File Offset: 0x0004E916
		public static bool TrainedTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 15000;
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x00243F8C File Offset: 0x0024218C
		public static string GetWildnessExplanation(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WildnessExplanation".Translate());
			stringBuilder.AppendLine();
			if (def.race != null && !def.race.Humanlike)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(def).ToStringTicksToDays("F1")));
			}
			if (!TrainableUtility.TamenessCanDecay(def))
			{
				stringBuilder.AppendLine("TamenessWillNotDecay".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004E8A RID: 20106
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		// Token: 0x04004E8B RID: 20107
		public const int MinTrainInterval = 15000;

		// Token: 0x04004E8C RID: 20108
		private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 12f),
				true
			},
			{
				new CurvePoint(1f, 6f),
				true
			}
		};
	}
}
