using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F42 RID: 3906
	public class RitualObligationTargetWorker_SkyLanterns : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06005CD9 RID: 23769 RVA: 0x001FE9E5 File Offset: 0x001FCBE5
		public RitualObligationTargetWorker_SkyLanterns()
		{
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x001FE9ED File Offset: 0x001FCBED
		public RitualObligationTargetWorker_SkyLanterns(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001FEC25 File Offset: 0x001FCE25
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Skylantern target"))
			{
				yield break;
			}
			List<Thing> ritualSpot = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			int num;
			for (int i = 0; i < ritualSpot.Count; i = num + 1)
			{
				yield return ritualSpot[i];
				num = i;
			}
			List<Thing> campfire = map.listerThings.ThingsOfDef(ThingDefOf.Campfire);
			for (int i = 0; i < campfire.Count; i = num + 1)
			{
				yield return campfire[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x001FEC38 File Offset: 0x001FCE38
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			if (thing.def != ThingDefOf.RitualSpot && thing.def != ThingDefOf.Campfire)
			{
				return false;
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, (float)this.def.unroofedCellSearchRadius, false))
			{
				if (intVec.InBounds(target.Map) && !intVec.Roofed(target.Map) && intVec.GetRoom(thing.Map) == room)
				{
					num++;
					if (num >= this.def.minUnroofedCells)
					{
						break;
					}
				}
			}
			if (num < this.def.minUnroofedCells)
			{
				return "RitualTargetNeedUnroofedCells".Translate(this.def.minUnroofedCells);
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.Campfire)
			{
				if (target.Cell.Roofed(target.Map))
				{
					return "RitualTargetCampfireMustBeUnroofed".Translate();
				}
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					if (compRefuelable.HasFuel)
					{
						return true;
					}
					return "RitualTargetCampfireNoFuel".Translate();
				}
			}
			return false;
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x001FEDDC File Offset: 0x001FCFDC
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetCampfireSkylanternsInfo".Translate();
			yield break;
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x001FEDE5 File Offset: 0x001FCFE5
		public override IEnumerable<string> GetBlockingIssues(TargetInfo target, IEnumerable<Pawn> participants)
		{
			Dictionary<Thing, int> dictionary = new Dictionary<Thing, int>();
			List<Thing> list = target.Map.listerThings.ThingsOfDef(ThingDefOf.WoodLog);
			bool flag = true;
			int num = 0;
			foreach (Pawn pawn in participants)
			{
				int num2 = Math.Max(this.def.woodPerParticipant - pawn.inventory.Count(ThingDefOf.WoodLog), 0);
				bool flag2 = false;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
					{
						int num3 = thing.stackCount;
						if (dictionary.ContainsKey(thing))
						{
							num3 = Math.Max(num3 - dictionary[thing], 0);
						}
						if (num3 >= num2)
						{
							if (dictionary.ContainsKey(thing))
							{
								dictionary[thing] += num2;
							}
							else
							{
								dictionary[thing] = num2;
							}
							num += 4;
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				yield return "RitualTargeWoodInfo".Translate(this.def.woodPerParticipant, num, this.def.woodPerParticipant * participants.Count<Pawn>());
			}
			yield break;
		}
	}
}
