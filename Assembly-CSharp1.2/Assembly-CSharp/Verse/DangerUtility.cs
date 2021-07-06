using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000A9 RID: 169
	public static class DangerUtility
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0008BB40 File Offset: 0x00089D40
		public static Danger NormalMaxDanger(this Pawn p)
		{
			if (p.CurJob != null && p.CurJob.playerForced)
			{
				return Danger.Deadly;
			}
			if (FloatMenuMakerMap.makingFor == p)
			{
				return Danger.Deadly;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				return Danger.Some;
			}
			if (p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Minor) && GenTemperature.FactionOwnsPassableRoomInTemperatureRange(p.Faction, p.SafeTemperatureRange(), p.MapHeld))
			{
				return Danger.None;
			}
			return Danger.Some;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0008BBAC File Offset: 0x00089DAC
		public static Danger GetDangerFor(this IntVec3 c, Pawn p, Map map)
		{
			Map mapHeld = p.MapHeld;
			if (mapHeld == null || mapHeld != map)
			{
				return Danger.None;
			}
			Region region = c.GetRegion(mapHeld, RegionType.Set_All);
			if (region == null)
			{
				return Danger.None;
			}
			return region.DangerFor(p);
		}
	}
}
