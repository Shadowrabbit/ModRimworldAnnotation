using System;
using RimWorld.Planet;

namespace Verse.Sound
{
	// Token: 0x02000950 RID: 2384
	public static class SoundStarter
	{
		// Token: 0x06003A7A RID: 14970 RVA: 0x00169FC0 File Offset: 0x001681C0
		public static void PlayOneShotOnCamera(this SoundDef soundDef, Map onlyThisMap = null)
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (onlyThisMap != null && (Find.CurrentMap != onlyThisMap || WorldRendererUtility.WorldRenderedNow))
			{
				return;
			}
			if (soundDef == null)
			{
				return;
			}
			if (soundDef.subSounds.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < soundDef.subSounds.Count; i++)
				{
					if (soundDef.subSounds[i].onCamera)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Log.Error("Tried to play " + soundDef + " on camera but it has no on-camera subSounds.", false);
				}
			}
			soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x0016A050 File Offset: 0x00168250
		public static void PlayOneShot(this SoundDef soundDef, SoundInfo info)
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (soundDef == null)
			{
				Log.Error("Tried to PlayOneShot with null SoundDef. Info=" + info, false);
				return;
			}
			DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
			if (soundDef == null)
			{
				return;
			}
			if (soundDef.isUndefined)
			{
				if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
				{
					DefDatabase<SoundDef>.Clear();
					DefDatabase<SoundDef>.AddAllInMods();
					SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
					if (!soundDef2.isUndefined)
					{
						soundDef2.PlayOneShot(info);
					}
				}
				return;
			}
			if (soundDef.sustain)
			{
				Log.Error("Tried to play sustainer SoundDef " + soundDef + " as a one-shot sound.", false);
				return;
			}
			if (!SoundSlotManager.CanPlayNow(soundDef.slot))
			{
				return;
			}
			for (int i = 0; i < soundDef.subSounds.Count; i++)
			{
				soundDef.subSounds[i].TryPlay(info);
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x0016A12C File Offset: 0x0016832C
		public static Sustainer TrySpawnSustainer(this SoundDef soundDef, SoundInfo info)
		{
			DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
			if (soundDef == null)
			{
				return null;
			}
			if (soundDef.isUndefined)
			{
				if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
				{
					DefDatabase<SoundDef>.Clear();
					DefDatabase<SoundDef>.AddAllInMods();
					SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
					if (!soundDef2.isUndefined)
					{
						return soundDef2.TrySpawnSustainer(info);
					}
				}
				return null;
			}
			if (!soundDef.sustain)
			{
				Log.Error("Tried to spawn a sustainer from non-sustainer sound " + soundDef + ".", false);
				return null;
			}
			if (!info.IsOnCamera && info.Maker.Thing != null && info.Maker.Thing.Destroyed)
			{
				return null;
			}
			if (soundDef.sustainStartSound != null)
			{
				soundDef.sustainStartSound.PlayOneShot(info);
			}
			return new Sustainer(soundDef, info);
		}
	}
}
