using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018B1 RID: 6321
	public static class ShipCountdown
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06008C47 RID: 35911 RVA: 0x0005E0E2 File Offset: 0x0005C2E2
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x0005E0F3 File Offset: 0x0005C2F3
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x0005E125 File Offset: 0x0005C325
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x0005E14C File Offset: 0x0005C34C
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

		// Token: 0x06008C4B RID: 35915 RVA: 0x0005E17B File Offset: 0x0005C37B
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x0028C608 File Offset: 0x0028A808
		private static void CountdownEnded()
		{
			if (ShipCountdown.shipRoot != null)
			{
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
				GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), stringBuilder.ToString()));
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
				GameVictoryUtility.ShowCredits(ShipCountdown.customLaunchString);
				return;
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), null));
		}

		// Token: 0x040059CD RID: 22989
		private static float timeLeft = -1000f;

		// Token: 0x040059CE RID: 22990
		private static Building shipRoot;

		// Token: 0x040059CF RID: 22991
		private static string customLaunchString;

		// Token: 0x040059D0 RID: 22992
		private const float InitialTime = 7.2f;
	}
}
