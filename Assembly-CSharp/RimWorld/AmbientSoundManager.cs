using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200162A RID: 5674
	public static class AmbientSoundManager
	{
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06007B54 RID: 31572 RVA: 0x00052E93 File Offset: 0x00051093
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06007B55 RID: 31573 RVA: 0x00052EA9 File Offset: 0x000510A9
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x06007B56 RID: 31574 RVA: 0x00052EBF File Offset: 0x000510BF
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x06007B57 RID: 31575 RVA: 0x00052ED9 File Offset: 0x000510D9
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(AmbientSoundManager.recreateMapSustainers);
		}

		// Token: 0x06007B58 RID: 31576 RVA: 0x002507B8 File Offset: 0x0024E9B8
		private static void RecreateMapSustainers()
		{
			if (!AmbientSoundManager.AltitudeWindSoundCreated)
			{
				SoundDefOf.Ambient_AltitudeWind.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
			SustainerManager sustainerManager = Find.SoundRoot.sustainerManager;
			for (int i = 0; i < AmbientSoundManager.biomeAmbientSustainers.Count; i++)
			{
				Sustainer sustainer = AmbientSoundManager.biomeAmbientSustainers[i];
				if (sustainerManager.AllSustainers.Contains(sustainer) && !sustainer.Ended)
				{
					sustainer.End();
				}
			}
			AmbientSoundManager.biomeAmbientSustainers.Clear();
			if (Find.CurrentMap != null)
			{
				List<SoundDef> soundsAmbient = Find.CurrentMap.Biome.soundsAmbient;
				for (int j = 0; j < soundsAmbient.Count; j++)
				{
					Sustainer item = soundsAmbient[j].TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
					AmbientSoundManager.biomeAmbientSustainers.Add(item);
				}
			}
		}

		// Token: 0x040050C2 RID: 20674
		private static List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x040050C3 RID: 20675
		private static Action recreateMapSustainers = new Action(AmbientSoundManager.RecreateMapSustainers);
	}
}
