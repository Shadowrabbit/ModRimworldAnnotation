using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200106B RID: 4203
	public static class ArchonexusCountdown
	{
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06006393 RID: 25491 RVA: 0x0021A228 File Offset: 0x00218428
		public static bool CountdownActivated
		{
			get
			{
				return ArchonexusCountdown.timeLeft > 0f;
			}
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x0021A236 File Offset: 0x00218436
		public static void InitiateCountdown(Building_ArchonexusCore archonexusCore)
		{
			if (!ModLister.CheckIdeology("Archonexus victory countdown"))
			{
				return;
			}
			ArchonexusCountdown.archonexusCoreRoot = archonexusCore;
			ArchonexusCountdown.timeLeft = 6f;
			SoundDefOf.Archotech_Invoked.PlayOneShot(archonexusCore);
			ScreenFader.StartFade(Color.white, 6f);
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x0021A274 File Offset: 0x00218474
		public static void CancelCountdown()
		{
			ArchonexusCountdown.timeLeft = -1f;
			ArchonexusCountdown.archonexusCoreRoot = null;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x0021A290 File Offset: 0x00218490
		private static void EndGame()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> list = (from p in ArchonexusCountdown.archonexusCoreRoot.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
			where p.RaceProps.Humanlike
			select p).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (!pawn.Dead && !pawn.IsQuestLodger())
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
					Find.StoryWatcher.statsRecord.colonistsLaunched++;
				}
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverArchotechInvokedIntro".Translate(), "GameOverArchotechInvokedEnding".Translate(), stringBuilder.ToString(), "GameOverColonistsTranscended", list), SongDefOf.ArchonexusVictorySong, true, 2.5f);
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x0021A3A0 File Offset: 0x002185A0
		public static void ArchonexusCountdownUpdate()
		{
			if (ArchonexusCountdown.timeLeft > 0f)
			{
				ArchonexusCountdown.timeLeft -= Time.deltaTime;
				if (ArchonexusCountdown.timeLeft <= 0f)
				{
					ArchonexusCountdown.EndGame();
				}
			}
		}

		// Token: 0x0400385E RID: 14430
		private const float ScreenFadeSeconds = 6f;

		// Token: 0x0400385F RID: 14431
		private const float SongStartDelay = 2.5f;

		// Token: 0x04003860 RID: 14432
		private static float timeLeft = -1f;

		// Token: 0x04003861 RID: 14433
		private static Building_ArchonexusCore archonexusCoreRoot;
	}
}
