using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F9 RID: 6137
	[Obsolete]
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06008F37 RID: 36663 RVA: 0x003350BB File Offset: 0x003332BB
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06008F38 RID: 36664 RVA: 0x003350C9 File Offset: 0x003332C9
		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					return "";
				}
				return TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
			}
		}

		// Token: 0x06008F39 RID: 36665 RVA: 0x003350E4 File Offset: 0x003332E4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06008F3A RID: 36666 RVA: 0x003350FE File Offset: 0x003332FE
		public void ResetForceExitAndRemoveMapCountdown()
		{
			this.ticksLeftToForceExitAndRemoveMap = -1;
		}

		// Token: 0x06008F3B RID: 36667 RVA: 0x00335107 File Offset: 0x00333307
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06008F3C RID: 36668 RVA: 0x00335114 File Offset: 0x00333314
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06008F3D RID: 36669 RVA: 0x0033511D File Offset: 0x0033331D
		public override string CompInspectStringExtra()
		{
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				return "ForceExitAndRemoveMapCountdown".Translate(this.ForceExitAndRemoveMapCountdownTimeLeftString) + ".";
			}
			return null;
		}

		// Token: 0x06008F3E RID: 36670 RVA: 0x00335150 File Offset: 0x00333350
		public override void CompTick()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (mapParent.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap <= 0)
					{
						TimedForcedExit.ForceReform(mapParent);
						return;
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		// Token: 0x06008F3F RID: 36671 RVA: 0x00335014 File Offset: 0x00333214
		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x06008F40 RID: 36672 RVA: 0x003351A0 File Offset: 0x003333A0
		public static void ForceReform(MapParent mapParent)
		{
			if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
			{
				Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
				Current.Game.CurrentMap = mapParent.Map;
				Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
				{
					if (mapParent.HasMap)
					{
						mapParent.Destroy();
					}
				}, true);
				Find.WindowStack.Add(window);
				return;
			}
			TimedForcedExit.tmpPawns.Clear();
			TimedForcedExit.tmpPawns.AddRange(from x in mapParent.Map.mapPawns.AllPawns
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			if (TimedForcedExit.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				CaravanExitMapUtility.ExitMapAndCreateCaravan(TimedForcedExit.tmpPawns, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, -1, true);
			}
			TimedForcedExit.tmpPawns.Clear();
			mapParent.Destroy();
		}

		// Token: 0x04005A16 RID: 23062
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x04005A17 RID: 23063
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
