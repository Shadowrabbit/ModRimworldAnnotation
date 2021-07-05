using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001577 RID: 5495
	public class CompRitualHediffGiverInRoom : ThingComp
	{
		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x060081ED RID: 33261 RVA: 0x002DEA53 File Offset: 0x002DCC53
		private CompProperties_RitualHediffGiverInRoom Props
		{
			get
			{
				return (CompProperties_RitualHediffGiverInRoom)this.props;
			}
		}

		// Token: 0x060081EE RID: 33262 RVA: 0x002DEA60 File Offset: 0x002DCC60
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.IsRitualTarget())
			{
				Room room = this.parent.GetRoom(RegionType.Set_All);
				if (room == null || room.TouchesMapEdge)
				{
					return;
				}
				foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (pawn.RaceProps.IsFlesh && pawn.GetRoom(RegionType.Set_All) == room && this.parent.Position.InHorDistOf(pawn.Position, this.Props.minRadius))
					{
						Hediff hediff = HediffMaker.MakeHediff(this.Props.hediff, pawn, null);
						if (this.Props.severity > 0f)
						{
							hediff.Severity = this.Props.severity;
						}
						pawn.health.AddHediff(hediff, null, null, null);
						if (this.Props.resetLastRecreationalDrugTick && pawn.mindState != null)
						{
							pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
						}
					}
				}
			}
		}

		// Token: 0x040050D7 RID: 20695
		private const int CheckInterval = 60;
	}
}
