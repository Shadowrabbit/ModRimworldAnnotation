using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA7 RID: 3751
	public static class TrainableUtility
	{
		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x001DFD87 File Offset: 0x001DDF87
		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x001DFD90 File Offset: 0x001DDF90
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

		// Token: 0x06005840 RID: 22592 RVA: 0x001DFE57 File Offset: 0x001DE057
		public static string MasterString(Pawn pawn)
		{
			if (pawn.playerSettings.Master == null)
			{
				return "(" + "NoneLower".TranslateSimple() + ")";
			}
			return RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x001DFE91 File Offset: 0x001DE091
		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x001DFEA4 File Offset: 0x001DE0A4
		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, Pawn>(rect, pawn, new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu), TrainableUtility.MasterString(pawn).Truncate(rect.width, null), null, TrainableUtility.MasterString(pawn), null, null, paintable);
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x001DFEED File Offset: 0x001DE0ED
		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x001DFEFA File Offset: 0x001DE0FA
		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
				{
					p.playerSettings.Master = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
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
						option = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = col
					};
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x001DFF0C File Offset: 0x001DE10C
		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				return false;
			}
			if (ModsConfig.IdeologyActive && animal.RaceProps.animalType == AnimalType.Dryad && animal.connections != null)
			{
				foreach (Thing thing in animal.connections.ConnectedThings)
				{
					CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
					if (compTreeConnection != null && compTreeConnection.ConnectedPawn == master)
					{
						return true;
					}
				}
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

		// Token: 0x06005846 RID: 22598 RVA: 0x001DFFE0 File Offset: 0x001DE1E0
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
				select bond.LabelShort).ToCommaList(true, false));
			}
			return text.TrimEndNewlines();
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x001E009C File Offset: 0x001DE29C
		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonist
			select bond.otherPawn;
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x001E00FC File Offset: 0x001DE2FC
		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.race.wildness) * 60000f);
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x001E011E File Offset: 0x001DE31E
		public static bool TamenessCanDecay(ThingDef def)
		{
			return !def.race.FenceBlocked && def.race.wildness > 0.101f;
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x001E0141 File Offset: 0x001DE341
		public static bool TrainedTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 15000;
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x001E0160 File Offset: 0x001DE360
		public static string GetWildnessExplanation(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WildnessExplanation".Translate());
			stringBuilder.AppendLine();
			if (def.race != null && !def.race.Humanlike)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(def).ToStringTicksToDays("F1")));
				stringBuilder.AppendLine();
			}
			if (!TrainableUtility.TamenessCanDecay(def))
			{
				string key = def.race.FenceBlocked ? "TamenessWillNotDecayFenceBlocked" : "TamenessWillNotDecay";
				stringBuilder.AppendLine(key.Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040033E6 RID: 13286
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		// Token: 0x040033E7 RID: 13287
		public const int MinTrainInterval = 15000;

		// Token: 0x040033E8 RID: 13288
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
