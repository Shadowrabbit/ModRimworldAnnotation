using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000545 RID: 1349
	public static class ReverbCustomPresets
	{
		// Token: 0x06002879 RID: 10361 RVA: 0x000F68FC File Offset: 0x000F4AFC
		public static void SetupAs(this ReverbSetup setup, AudioReverbPreset preset)
		{
			GameObject gameObject = new GameObject("OneFrameTemp");
			gameObject.AddComponent<AudioSource>();
			AudioReverbFilter audioReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
			audioReverbFilter.reverbPreset = preset;
			setup.dryLevel = audioReverbFilter.dryLevel;
			setup.room = audioReverbFilter.room;
			setup.roomHF = audioReverbFilter.roomHF;
			setup.roomLF = audioReverbFilter.roomLF;
			setup.decayTime = audioReverbFilter.decayTime;
			setup.decayHFRatio = audioReverbFilter.decayHFRatio;
			setup.reflectionsLevel = audioReverbFilter.reflectionsLevel;
			setup.reflectionsDelay = audioReverbFilter.reflectionsDelay;
			setup.reverbLevel = audioReverbFilter.reverbLevel;
			setup.reverbDelay = audioReverbFilter.reverbDelay;
			setup.hfReference = audioReverbFilter.hfReference;
			setup.lfReference = audioReverbFilter.lfReference;
			setup.diffusion = audioReverbFilter.diffusion;
			setup.density = audioReverbFilter.density;
			UnityEngine.Object.Destroy(audioReverbFilter);
		}
	}
}
