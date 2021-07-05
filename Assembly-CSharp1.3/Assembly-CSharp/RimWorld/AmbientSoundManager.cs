using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001020 RID: 4128
	public static class AmbientSoundManager
	{
		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06006170 RID: 24944 RVA: 0x0021181F File Offset: 0x0020FA1F
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06006171 RID: 24945 RVA: 0x00211835 File Offset: 0x0020FA35
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x0021184B File Offset: 0x0020FA4B
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x00211865 File Offset: 0x0020FA65
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(AmbientSoundManager.recreateMapSustainers);
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00211874 File Offset: 0x0020FA74
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

		// Token: 0x04003781 RID: 14209
		private static List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x04003782 RID: 14210
		private static Action recreateMapSustainers = new Action(AmbientSoundManager.RecreateMapSustainers);
	}
}
