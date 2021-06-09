using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B2 RID: 6322
	public static class GameVictoryUtility
	{
		// Token: 0x06008C4E RID: 35918 RVA: 0x0028C778 File Offset: 0x0028A978
		public static string MakeEndCredits(string intro, string ending, string escapees)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(intro);
			if (!escapees.NullOrEmpty())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append("GameOverColonistsEscaped".Translate(escapees));
			}
			stringBuilder.AppendLine();
			string text = GameVictoryUtility.PawnsLeftBehind();
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine("GameOverColonistsLeft".Translate(text));
			}
			stringBuilder.AppendLine(ending);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(GameVictoryUtility.InMemoryOfSection());
			return stringBuilder.ToString();
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x0028C818 File Offset: 0x0028AA18
		public static void ShowCredits(string victoryText)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText);
			screen_Credits.wonGame = true;
			Find.WindowStack.Add(screen_Credits);
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x06008C50 RID: 35920 RVA: 0x0028C85C File Offset: 0x0028AA5C
		public static string PawnsLeftBehind()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				stringBuilder.AppendLine("   " + pawn.LabelCap);
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
						if (pawn2.IsColonist && pawn2.HostFaction == null)
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

		// Token: 0x06008C51 RID: 35921 RVA: 0x0028C960 File Offset: 0x0028AB60
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
