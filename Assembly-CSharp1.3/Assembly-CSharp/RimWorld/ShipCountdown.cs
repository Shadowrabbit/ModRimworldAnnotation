using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011DB RID: 4571
	public static class ShipCountdown
	{
		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06006E4F RID: 28239 RVA: 0x0024F89C File Offset: 0x0024DA9C
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06006E50 RID: 28240 RVA: 0x0024F8AD File Offset: 0x0024DAAD
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06006E51 RID: 28241 RVA: 0x0024F8DF File Offset: 0x0024DADF
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06006E52 RID: 28242 RVA: 0x0024F906 File Offset: 0x0024DB06
		public static void ShipCountdownUpdate()
		{
			if (ShipCountdown.timeLeft > 0f)
			{
				ShipCountdown.timeLeft -= Time.deltaTime;
				if (ShipCountdown.timeLeft <= 0f)
				{
					ShipCountdown.CountdownEnded();
				}
			}
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x0024F935 File Offset: 0x0024DB35
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x0024F94C File Offset: 0x0024DB4C
		private static void CountdownEnded()
		{
			if (ShipCountdown.shipRoot != null)
			{
				TaggedString taggedString = "GameOverShipLaunchedEnding".Translate();
				List<Building> list = ShipUtility.ShipBuildingsAttachedTo(ShipCountdown.shipRoot).ToList<Building>();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Building building in list)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						stringBuilder.AppendLine("   " + building_CryptosleepCasket.ContainedThing.LabelCap);
						Find.StoryWatcher.statsRecord.colonistsLaunched++;
						TaleRecorder.RecordTale(TaleDefOf.LaunchedShip, new object[]
						{
							building_CryptosleepCasket.ContainedThing
						});
					}
				}
				GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), taggedString, stringBuilder.ToString(), "GameOverColonistsEscaped", null), null, false, 5f);
				using (List<Building>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Building building2 = enumerator.Current;
						building2.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			if (!ShipCountdown.customLaunchString.NullOrEmpty())
			{
				GameVictoryUtility.ShowCredits(ShipCountdown.customLaunchString, null, false, 5f);
				return;
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), null, "GameOverColonistsEscaped", null), null, false, 5f);
		}

		// Token: 0x04003D2F RID: 15663
		private static float timeLeft = -1000f;

		// Token: 0x04003D30 RID: 15664
		private static Building shipRoot;

		// Token: 0x04003D31 RID: 15665
		private static string customLaunchString;

		// Token: 0x04003D32 RID: 15666
		private const float InitialTime = 7.2f;
	}
}
