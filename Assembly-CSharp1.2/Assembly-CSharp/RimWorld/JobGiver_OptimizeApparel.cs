using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CDA RID: 3290
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		// Token: 0x06004BE2 RID: 19426 RVA: 0x00036085 File Offset: 0x00034285
		private void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x001A75C8 File Offset: 0x001A57C8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.outfits == null)
			{
				Log.ErrorOnce(pawn + " tried to run JobGiver_OptimizeApparel without an OutfitTracker", 5643897, false);
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				Log.ErrorOnce("Non-colonist " + pawn + " tried to optimize apparel.", 764323, false);
				return null;
			}
			if (pawn.IsQuestLodger())
			{
				return null;
			}
			if (!DebugViewSettings.debugApparelOptimize)
			{
				if (Find.TickManager.TicksGame < pawn.mindState.nextApparelOptimizeTick)
				{
					return null;
				}
			}
			else
			{
				JobGiver_OptimizeApparel.debugSb = new StringBuilder();
				JobGiver_OptimizeApparel.debugSb.AppendLine(string.Concat(new object[]
				{
					"Scanning for ",
					pawn,
					" at ",
					pawn.Position
				}));
			}
			Outfit currentOutfit = pawn.outfits.CurrentOutfit;
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!currentOutfit.filter.Allows(wornApparel[i]) && pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]) && !pawn.apparel.IsLocked(wornApparel[i]))
				{
					Job job = JobMaker.MakeJob(JobDefOf.RemoveApparel, wornApparel[i]);
					job.haulDroppedApparel = true;
					return job;
				}
			}
			Thing thing = null;
			float num = 0f;
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
			if (list.Count == 0)
			{
				this.SetNextOptimizeTick(pawn);
				return null;
			}
			JobGiver_OptimizeApparel.neededWarmth = PawnApparelGenerator.CalculateNeededWarmth(pawn, pawn.Map.Tile, GenLocalDate.Twelfth(pawn));
			JobGiver_OptimizeApparel.wornApparelScores.Clear();
			for (int j = 0; j < wornApparel.Count; j++)
			{
				JobGiver_OptimizeApparel.wornApparelScores.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, wornApparel[j]));
			}
			for (int k = 0; k < list.Count; k++)
			{
				Apparel apparel = (Apparel)list[k];
				if (currentOutfit.filter.Allows(apparel) && apparel.IsInAnyStorage() && !apparel.IsForbidden(pawn) && !apparel.IsBurning() && (apparel.def.apparel.gender == Gender.None || apparel.def.apparel.gender == pawn.gender))
				{
					float num2 = JobGiver_OptimizeApparel.ApparelScoreGain_NewTmp(pawn, apparel, JobGiver_OptimizeApparel.wornApparelScores);
					if (DebugViewSettings.debugApparelOptimize)
					{
						JobGiver_OptimizeApparel.debugSb.AppendLine(apparel.LabelCap + ": " + num2.ToString("F2"));
					}
					if (num2 >= 0.05f && num2 >= num && (!EquipmentUtility.IsBiocoded(apparel) || EquipmentUtility.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def) && pawn.CanReserveAndReach(apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
					{
						thing = apparel;
						num = num2;
					}
				}
			}
			if (DebugViewSettings.debugApparelOptimize)
			{
				JobGiver_OptimizeApparel.debugSb.AppendLine("BEST: " + thing);
				Log.Message(JobGiver_OptimizeApparel.debugSb.ToString(), false);
				JobGiver_OptimizeApparel.debugSb = null;
			}
			if (thing == null)
			{
				this.SetNextOptimizeTick(pawn);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wear, thing);
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x001A7910 File Offset: 0x001A5B10
		public static float ApparelScoreGain_NewTmp(Pawn pawn, Apparel ap, List<float> wornScoresCache)
		{
			if (ap is ShieldBelt && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				return -1000f;
			}
			float num = JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, ap);
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			bool flag = false;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(wornApparel[i].def, ap.def, pawn.RaceProps.body))
				{
					if (!pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]) || pawn.apparel.IsLocked(wornApparel[i]))
					{
						return -1000f;
					}
					num -= wornScoresCache[i];
					flag = true;
				}
			}
			if (!flag)
			{
				num *= 10f;
			}
			return num;
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x001A79E8 File Offset: 0x001A5BE8
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static float ApparelScoreGain(Pawn pawn, Apparel ap)
		{
			JobGiver_OptimizeApparel.wornApparelScores.Clear();
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				JobGiver_OptimizeApparel.wornApparelScores.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, pawn.apparel.WornApparel[i]));
			}
			return JobGiver_OptimizeApparel.ApparelScoreGain_NewTmp(pawn, ap, JobGiver_OptimizeApparel.wornApparelScores);
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x001A7A48 File Offset: 0x001A5C48
		public static float ApparelScoreRaw(Pawn pawn, Apparel ap)
		{
			float num = 0.1f + ap.def.apparel.scoreOffset;
			float num2 = ap.GetStatValue(StatDefOf.ArmorRating_Sharp, true) + ap.GetStatValue(StatDefOf.ArmorRating_Blunt, true) + ap.GetStatValue(StatDefOf.ArmorRating_Heat, true);
			num += num2;
			if (ap.def.useHitPoints)
			{
				float x = (float)ap.HitPoints / (float)ap.MaxHitPoints;
				num *= JobGiver_OptimizeApparel.HitPointsPercentScoreFactorCurve.Evaluate(x);
			}
			num += ap.GetSpecialApparelScoreOffset();
			float num3 = 1f;
			if (JobGiver_OptimizeApparel.neededWarmth == NeededWarmth.Warm)
			{
				float statValue = ap.GetStatValue(StatDefOf.Insulation_Cold, true);
				num3 *= JobGiver_OptimizeApparel.InsulationColdScoreFactorCurve_NeedWarm.Evaluate(statValue);
			}
			num *= num3;
			if (ap.WornByCorpse && (pawn == null || ThoughtUtility.CanGetThought_NewTemp(pawn, ThoughtDefOf.DeadMansApparel, true)))
			{
				num -= 0.5f;
				if (num > 0f)
				{
					num *= 0.1f;
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn == null || ThoughtUtility.CanGetThought_NewTemp(pawn, ThoughtDefOf.HumanLeatherApparelSad, true))
				{
					num -= 0.5f;
					if (num > 0f)
					{
						num *= 0.1f;
					}
				}
				if (pawn != null && ThoughtUtility.CanGetThought_NewTemp(pawn, ThoughtDefOf.HumanLeatherApparelHappy, true))
				{
					num += 0.12f;
				}
			}
			if (pawn != null && !ap.def.apparel.CorrectGenderForWearing(pawn.gender))
			{
				num *= 0.01f;
			}
			if (pawn != null && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				JobGiver_OptimizeApparel.tmpAllowedApparels.Clear();
				JobGiver_OptimizeApparel.tmpRequiredApparels.Clear();
				JobGiver_OptimizeApparel.tmpBodyPartGroupsWithRequirement.Clear();
				QualityCategory qualityCategory = QualityCategory.Awful;
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.def.requiredApparel != null)
					{
						for (int i = 0; i < royalTitle.def.requiredApparel.Count; i++)
						{
							JobGiver_OptimizeApparel.tmpAllowedApparels.AddRange(royalTitle.def.requiredApparel[i].AllAllowedApparelForPawn(pawn, false, true));
							JobGiver_OptimizeApparel.tmpRequiredApparels.AddRange(royalTitle.def.requiredApparel[i].AllRequiredApparelForPawn(pawn, false, true));
							JobGiver_OptimizeApparel.tmpBodyPartGroupsWithRequirement.AddRange(royalTitle.def.requiredApparel[i].bodyPartGroupsMatchAny);
						}
					}
					if (royalTitle.def.requiredMinimumApparelQuality > qualityCategory)
					{
						qualityCategory = royalTitle.def.requiredMinimumApparelQuality;
					}
				}
				bool flag = ap.def.apparel.bodyPartGroups.Any((BodyPartGroupDef bp) => JobGiver_OptimizeApparel.tmpBodyPartGroupsWithRequirement.Contains(bp));
				QualityCategory qualityCategory2;
				if (ap.TryGetQuality(out qualityCategory2) && qualityCategory2 < qualityCategory)
				{
					num *= 0.25f;
				}
				if (flag)
				{
					foreach (ThingDef item in JobGiver_OptimizeApparel.tmpRequiredApparels)
					{
						JobGiver_OptimizeApparel.tmpAllowedApparels.Remove(item);
					}
					if (JobGiver_OptimizeApparel.tmpAllowedApparels.Contains(ap.def))
					{
						num *= 10f;
					}
					if (JobGiver_OptimizeApparel.tmpRequiredApparels.Contains(ap.def))
					{
						num *= 25f;
					}
				}
			}
			return num;
		}

		// Token: 0x04003206 RID: 12806
		private static NeededWarmth neededWarmth;

		// Token: 0x04003207 RID: 12807
		private static StringBuilder debugSb;

		// Token: 0x04003208 RID: 12808
		private static List<float> wornApparelScores = new List<float>();

		// Token: 0x04003209 RID: 12809
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		// Token: 0x0400320A RID: 12810
		private const int ApparelOptimizeCheckIntervalMax = 9000;

		// Token: 0x0400320B RID: 12811
		private const float MinScoreGainToCare = 0.05f;

		// Token: 0x0400320C RID: 12812
		private const float ScoreFactorIfNotReplacing = 10f;

		// Token: 0x0400320D RID: 12813
		private static readonly SimpleCurve InsulationColdScoreFactorCurve_NeedWarm = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 8f),
				true
			}
		};

		// Token: 0x0400320E RID: 12814
		private static readonly SimpleCurve HitPointsPercentScoreFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.2f, 0.2f),
				true
			},
			{
				new CurvePoint(0.22f, 0.6f),
				true
			},
			{
				new CurvePoint(0.5f, 0.6f),
				true
			},
			{
				new CurvePoint(0.52f, 1f),
				true
			}
		};

		// Token: 0x0400320F RID: 12815
		private static HashSet<BodyPartGroupDef> tmpBodyPartGroupsWithRequirement = new HashSet<BodyPartGroupDef>();

		// Token: 0x04003210 RID: 12816
		private static HashSet<ThingDef> tmpAllowedApparels = new HashSet<ThingDef>();

		// Token: 0x04003211 RID: 12817
		private static HashSet<ThingDef> tmpRequiredApparels = new HashSet<ThingDef>();
	}
}
