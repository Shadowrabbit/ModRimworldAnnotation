using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7B RID: 3963
	public class RitualOutcomeEffectWorker_Skylantern : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DE9 RID: 24041 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Skylantern()
		{
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Skylantern(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x00203AE4 File Offset: 0x00201CE4
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.2f))
			{
				List<Thing> list = new List<Thing>();
				Ideo ideo = jobRitual.Ritual.ideo;
				using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Building precept_Building;
						if ((precept_Building = (enumerator.Current as Precept_Building)) != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable)
						{
							list.Add(ThingMaker.MakeThing(precept_Building.ThingDef, null).MakeMinified());
						}
					}
				}
				if (list.Count == 0)
				{
					list.AddRange(ThingSetMakerDefOf.VisitorGift.root.Generate(new ThingSetMakerParams
					{
						totalMarketValueRange = new FloatRange?(RitualOutcomeEffectWorker_Skylantern.RewardMarketValueRange)
					}));
				}
				IncidentParms incidentParms = new IncidentParms
				{
					target = jobRitual.Map,
					points = 160f,
					gifts = list,
					pawnIdeo = ideo,
					pawnCount = 3,
					storeGeneratedNeutralPawns = new List<Pawn>()
				};
				if (IncidentDefOf.WanderersSkylantern.Worker.TryExecute(incidentParms))
				{
					List<GlobalTargetInfo> list2 = new List<GlobalTargetInfo>();
					list2.AddRange(letterLookTargets.targets);
					list2.AddRange(from p in incidentParms.storeGeneratedNeutralPawns
					select new GlobalTargetInfo(p));
					letterLookTargets = new LookTargets(list2);
					extraOutcomeDesc = "RitualOutcomeExtraDesc_SkylanternWanderers".Translate();
				}
			}
		}

		// Token: 0x04003635 RID: 13877
		public const float WanderersChance = 0.2f;

		// Token: 0x04003636 RID: 13878
		public const float WanderersPoints = 160f;

		// Token: 0x04003637 RID: 13879
		public static readonly FloatRange RewardMarketValueRange = new FloatRange(200f, 500f);
	}
}
