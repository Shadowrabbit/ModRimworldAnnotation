using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020006A2 RID: 1698
	public class DebugOutputsWorldPawns
	{
		// Token: 0x06002C47 RID: 11335 RVA: 0x0012E900 File Offset: 0x0012CB00
		[DebugOutput("World pawns", true)]
		public static void ColonistRelativeChance()
		{
			HashSet<Pawn> hashSet = new HashSet<Pawn>(Find.WorldPawns.AllPawnsAliveOrDead);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < 500; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				list.Add(pawn);
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
				}
			}
			int num = list.Count((Pawn x) => PawnRelationUtility.GetMostImportantColonyRelative(x) != null);
			Log.Message(string.Concat(new object[]
			{
				"Colony relatives: ",
				((float)num / 500f).ToStringPercent(),
				" (",
				num,
				" of ",
				500,
				")"
			}), false);
			foreach (Pawn pawn2 in Find.WorldPawns.AllPawnsAliveOrDead.ToList<Pawn>())
			{
				if (!hashSet.Contains(pawn2))
				{
					Find.WorldPawns.RemovePawn(pawn2);
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
				}
			}
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x0002335F File Offset: 0x0002155F
		[DebugOutput("World pawns", true)]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x0002336B File Offset: 0x0002156B
		[DebugOutput("World pawns", true)]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00023377 File Offset: 0x00021577
		[DebugOutput("World pawns", true)]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00023383 File Offset: 0x00021583
		[DebugOutput("World pawns", true)]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00023394 File Offset: 0x00021594
		[DebugOutput("World pawns", true)]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000233A5 File Offset: 0x000215A5
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000233B6 File Offset: 0x000215B6
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}
	}
}
