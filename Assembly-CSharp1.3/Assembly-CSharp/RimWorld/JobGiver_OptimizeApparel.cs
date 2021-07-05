using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C8 RID: 1992
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		// Token: 0x060035B7 RID: 13751 RVA: 0x0012F940 File Offset: 0x0012DB40
		private static void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x0012F968 File Offset: 0x0012DB68
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.outfits == null)
			{
				Log.ErrorOnce(pawn + " tried to run JobGiver_OptimizeApparel without an OutfitTracker", 5643897);
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				Log.ErrorOnce("Non-colonist " + pawn + " tried to optimize apparel.", 764323);
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
			Job result;
			if (ModsConfig.IdeologyActive && JobGiver_OptimizeApparel.TryCreateRecolorJob(pawn, out result, false))
			{
				return result;
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
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
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
					float num2 = JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel, JobGiver_OptimizeApparel.wornApparelScores);
					if (DebugViewSettings.debugApparelOptimize)
					{
						JobGiver_OptimizeApparel.debugSb.AppendLine(apparel.LabelCap + ": " + num2.ToString("F2"));
					}
					if (num2 >= 0.05f && num2 >= num && (!CompBiocodable.IsBiocoded(apparel) || CompBiocodable.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def) && pawn.CanReserveAndReach(apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
					{
						thing = apparel;
						num = num2;
					}
				}
			}
			if (DebugViewSettings.debugApparelOptimize)
			{
				JobGiver_OptimizeApparel.debugSb.AppendLine("BEST: " + thing);
				Log.Message(JobGiver_OptimizeApparel.debugSb.ToString());
				JobGiver_OptimizeApparel.debugSb = null;
			}
			if (thing == null)
			{
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wear, thing);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x0012FCC0 File Offset: 0x0012DEC0
		public static bool TryCreateRecolorJob(Pawn pawn, out Job job, bool dryRun = false)
		{
			if (!ModLister.CheckIdeology("Apparel recoloring"))
			{
				job = null;
				return false;
			}
			if (pawn.apparel.AnyApparelNeedsRecoloring)
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.StylingStation), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false), null, 0, -1, false, RegionType.Set_Passable, false);
				if (thing != null)
				{
					try
					{
						foreach (Apparel apparel in pawn.apparel.WornApparel)
						{
							if (apparel.DesiredColor != null)
							{
								JobGiver_OptimizeApparel.tmpApparelToRecolor.Add(apparel);
							}
						}
						List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
						if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count > 0)
						{
							list.SortBy((Thing t) => t.Position.DistanceToSquared(pawn.Position));
							foreach (Thing thing2 in list)
							{
								if (pawn.CanReach(thing2, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && !thing2.IsForbidden(pawn))
								{
									int num = 0;
									while (num < thing2.stackCount && pawn.CanReserve(thing2, 1, num + 1, null, false))
									{
										JobGiver_OptimizeApparel.tmpQueueApparel.Add(JobGiver_OptimizeApparel.tmpApparelToRecolor[JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1]);
										if (!JobGiver_OptimizeApparel.tmpQueueDye.Contains(thing2))
										{
											JobGiver_OptimizeApparel.tmpQueueDye.Add(thing2);
										}
										JobGiver_OptimizeApparel.tmpApparelToRecolor.RemoveAt(JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1);
										if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
										{
											break;
										}
										num++;
									}
									if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
									{
										break;
									}
								}
							}
							if (JobGiver_OptimizeApparel.tmpQueueApparel.Count > 0)
							{
								if (dryRun)
								{
									job = null;
								}
								else
								{
									job = JobMaker.MakeJob(JobDefOf.RecolorApparel);
									List<LocalTargetInfo> targetQueue = job.GetTargetQueue(TargetIndex.A);
									List<LocalTargetInfo> targetQueue2 = job.GetTargetQueue(TargetIndex.B);
									targetQueue.AddRange(JobGiver_OptimizeApparel.tmpQueueDye);
									targetQueue2.AddRange(JobGiver_OptimizeApparel.tmpQueueApparel);
									job.SetTarget(TargetIndex.C, thing);
									job.count = JobGiver_OptimizeApparel.tmpQueueApparel.Count;
								}
								return true;
							}
						}
					}
					finally
					{
						JobGiver_OptimizeApparel.tmpApparelToRecolor.Clear();
						JobGiver_OptimizeApparel.tmpQueueApparel.Clear();
						JobGiver_OptimizeApparel.tmpQueueDye.Clear();
					}
				}
			}
			job = null;
			return false;
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x0012FFDC File Offset: 0x0012E1DC
		public static float ApparelScoreGain(Pawn pawn, Apparel ap, List<float> wornScoresCache)
		{
			if (ap is ShieldBelt && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				return -1000f;
			}
			if (ap.def.apparel.ignoredByNonViolent && pawn.WorkTagIsDisabled(WorkTags.Violent))
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

		// Token: 0x060035BB RID: 13755 RVA: 0x001300D4 File Offset: 0x0012E2D4
		public static float ApparelScoreRaw(Pawn pawn, Apparel ap)
		{
			float num = 0.1f + ap.def.apparel.scoreOffset;
			float num2 = ap.GetStatValue(StatDefOf.ArmorRating_Sharp, true) + ap.GetStatValue(StatDefOf.ArmorRating_Blunt, true);
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
			if (ap.WornByCorpse && (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.DeadMansApparel, true)))
			{
				num -= 0.5f;
				if (num > 0f)
				{
					num *= 0.1f;
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelSad, true))
				{
					num -= 0.5f;
					if (num > 0f)
					{
						num *= 0.1f;
					}
				}
				if (pawn != null && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelHappy, true))
				{
					num += 0.12f;
				}
			}
			if (pawn != null && !ap.def.apparel.CorrectGenderForWearing(pawn.gender))
			{
				num *= 0.01f;
			}
			bool flag = false;
			if (pawn != null)
			{
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in pawn.apparel.AllRequirements)
				{
					foreach (BodyPartGroupDef item in apparelRequirementWithSource.requirement.bodyPartGroupsMatchAny)
					{
						if (ap.def.apparel.bodyPartGroups.Contains(item))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				foreach (ApparelRequirementWithSource apparelRequirementWithSource2 in pawn.apparel.AllRequirements)
				{
					if (apparelRequirementWithSource2.requirement.RequiredForPawn(pawn, ap.def, false))
					{
						flag2 = true;
					}
					if (apparelRequirementWithSource2.requirement.AllowedForPawn(pawn, ap.def, false))
					{
						flag3 = true;
					}
				}
				if (flag2)
				{
					num *= 25f;
				}
				else if (flag3)
				{
					num *= 10f;
				}
			}
			if (pawn != null && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				QualityCategory qualityCategory = QualityCategory.Awful;
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.def.requiredMinimumApparelQuality > qualityCategory)
					{
						qualityCategory = royalTitle.def.requiredMinimumApparelQuality;
					}
				}
				QualityCategory qualityCategory2;
				if (ap.TryGetQuality(out qualityCategory2) && qualityCategory2 < qualityCategory)
				{
					num *= 0.25f;
				}
			}
			if (ap.def.apparel.blocksVision)
			{
				num = -10f;
			}
			if (ap.def.apparel.slaveApparel && !pawn.IsSlave)
			{
				num = -10f;
			}
			return num;
		}

		// Token: 0x04001EAF RID: 7855
		private static NeededWarmth neededWarmth;

		// Token: 0x04001EB0 RID: 7856
		private static StringBuilder debugSb;

		// Token: 0x04001EB1 RID: 7857
		private static List<float> wornApparelScores = new List<float>();

		// Token: 0x04001EB2 RID: 7858
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		// Token: 0x04001EB3 RID: 7859
		private const int ApparelOptimizeCheckIntervalMax = 9000;

		// Token: 0x04001EB4 RID: 7860
		private const float MinScoreGainToCare = 0.05f;

		// Token: 0x04001EB5 RID: 7861
		private const float ScoreFactorIfNotReplacing = 10f;

		// Token: 0x04001EB6 RID: 7862
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

		// Token: 0x04001EB7 RID: 7863
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
				new CurvePoint(0.22f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 0.3f),
				true
			},
			{
				new CurvePoint(0.52f, 1f),
				true
			}
		};

		// Token: 0x04001EB8 RID: 7864
		private static List<LocalTargetInfo> tmpQueueDye = new List<LocalTargetInfo>();

		// Token: 0x04001EB9 RID: 7865
		private static List<LocalTargetInfo> tmpQueueApparel = new List<LocalTargetInfo>();

		// Token: 0x04001EBA RID: 7866
		private static List<Apparel> tmpApparelToRecolor = new List<Apparel>();
	}
}
