using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DC RID: 4572
	public static class GameVictoryUtility
	{
		// Token: 0x06006E56 RID: 28246 RVA: 0x0024FAF0 File Offset: 0x0024DCF0
		public static string MakeEndCredits(string intro, string ending, string escapees, string colonistsEscapeeTKey = "GameOverColonistsEscaped", IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(intro);
			if (!escapees.NullOrEmpty())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(colonistsEscapeeTKey.Translate(escapees));
			}
			stringBuilder.AppendLine();
			string text = GameVictoryUtility.PawnsLeftBehind(escaped);
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine("GameOverColonistsLeft".Translate(text));
			}
			stringBuilder.AppendLine(ending);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(GameVictoryUtility.InMemoryOfSection());
			return stringBuilder.ToString();
		}

		// Token: 0x06006E57 RID: 28247 RVA: 0x0024FB8C File Offset: 0x0024DD8C
		public static void ShowCredits(string victoryText, SongDef endCreditsSong = null, bool exitToMainMenu = false, float songStartDelay = 5f)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText);
			screen_Credits.wonGame = true;
			screen_Credits.endCreditsSong = endCreditsSong;
			screen_Credits.exitToMainMenu = exitToMainMenu;
			screen_Credits.songStartDelay = songStartDelay;
			Find.WindowStack.Add(screen_Credits);
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x0024FBE8 File Offset: 0x0024DDE8
		public static string PawnsLeftBehind(IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (escaped == null || !escaped.Contains(pawn))
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.IsPlayerControlled)
				{
					List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
					for (int j = 0; j < pawnsListForReading.Count; j++)
					{
						Pawn pawn2 = pawnsListForReading[j];
						if (pawn2.IsColonist && pawn2.HostFaction == null && (escaped == null || !escaped.Contains(pawn2)))
						{
							stringBuilder.AppendLine("   " + pawn2.LabelCap);
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x0024FD08 File Offset: 0x0024DF08
		public static string InMemoryOfSection()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from p in Find.WorldPawns.AllPawnsDead
			where p.IsColonist
			select p;
			if (enumerable.Any<Pawn>())
			{
				stringBuilder.AppendLine("GameOverInMemoryOf".Translate());
				foreach (Pawn pawn in enumerable)
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
