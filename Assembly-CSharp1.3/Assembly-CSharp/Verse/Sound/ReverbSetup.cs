using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000546 RID: 1350
	public class ReverbSetup
	{
		// Token: 0x0600287A RID: 10362 RVA: 0x000F69D8 File Offset: 0x000F4BD8
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AudioReverbPreset)))
				{
					AudioReverbPreset audioReverbPreset = (AudioReverbPreset)obj;
					if (audioReverbPreset != AudioReverbPreset.User)
					{
						AudioReverbPreset localPreset = audioReverbPreset;
						list.Add(new FloatMenuOption(audioReverbPreset.ToString(), delegate()
						{
							this.SetupAs(localPreset);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000F6ABC File Offset: 0x000F4CBC
		public void ApplyTo(AudioReverbFilter filter)
		{
			filter.dryLevel = this.dryLevel;
			filter.room = this.room;
			filter.roomHF = this.roomHF;
			filter.roomLF = this.roomLF;
			filter.decayTime = this.decayTime;
			filter.decayHFRatio = this.decayHFRatio;
			filter.reflectionsLevel = this.reflectionsLevel;
			filter.reflectionsDelay = this.reflectionsDelay;
			filter.reverbLevel = this.reverbLevel;
			filter.reverbDelay = this.reverbDelay;
			filter.hfReference = this.hfReference;
			filter.lfReference = this.lfReference;
			filter.diffusion = this.diffusion;
			filter.density = this.density;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000F6B74 File Offset: 0x000F4D74
		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			return new ReverbSetup
			{
				dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t),
				room = Mathf.Lerp(A.room, B.room, t),
				roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t),
				roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t),
				decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t),
				decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t),
				reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t),
				reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t),
				reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t),
				reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t),
				hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t),
				lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t),
				diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t),
				density = Mathf.Lerp(A.density, B.density, t)
			};
		}

		// Token: 0x040018F5 RID: 6389
		public float dryLevel;

		// Token: 0x040018F6 RID: 6390
		public float room;

		// Token: 0x040018F7 RID: 6391
		public float roomHF;

		// Token: 0x040018F8 RID: 6392
		public float roomLF;

		// Token: 0x040018F9 RID: 6393
		public float decayTime = 1f;

		// Token: 0x040018FA RID: 6394
		public float decayHFRatio = 0.5f;

		// Token: 0x040018FB RID: 6395
		public float reflectionsLevel = -10000f;

		// Token: 0x040018FC RID: 6396
		public float reflectionsDelay;

		// Token: 0x040018FD RID: 6397
		public float reverbLevel;

		// Token: 0x040018FE RID: 6398
		public float reverbDelay = 0.04f;

		// Token: 0x040018FF RID: 6399
		public float hfReference = 5000f;

		// Token: 0x04001900 RID: 6400
		public float lfReference = 250f;

		// Token: 0x04001901 RID: 6401
		public float diffusion = 100f;

		// Token: 0x04001902 RID: 6402
		public float density = 100f;
	}
}
