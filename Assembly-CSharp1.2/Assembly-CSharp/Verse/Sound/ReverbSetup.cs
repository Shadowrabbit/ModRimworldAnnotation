using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000918 RID: 2328
	public class ReverbSetup
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x00167DFC File Offset: 0x00165FFC
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, true))
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00167ED0 File Offset: 0x001660D0
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

		// Token: 0x060039B2 RID: 14770 RVA: 0x00167F88 File Offset: 0x00166188
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

		// Token: 0x040027E9 RID: 10217
		public float dryLevel;

		// Token: 0x040027EA RID: 10218
		public float room;

		// Token: 0x040027EB RID: 10219
		public float roomHF;

		// Token: 0x040027EC RID: 10220
		public float roomLF;

		// Token: 0x040027ED RID: 10221
		public float decayTime = 1f;

		// Token: 0x040027EE RID: 10222
		public float decayHFRatio = 0.5f;

		// Token: 0x040027EF RID: 10223
		public float reflectionsLevel = -10000f;

		// Token: 0x040027F0 RID: 10224
		public float reflectionsDelay;

		// Token: 0x040027F1 RID: 10225
		public float reverbLevel;

		// Token: 0x040027F2 RID: 10226
		public float reverbDelay = 0.04f;

		// Token: 0x040027F3 RID: 10227
		public float hfReference = 5000f;

		// Token: 0x040027F4 RID: 10228
		public float lfReference = 250f;

		// Token: 0x040027F5 RID: 10229
		public float diffusion = 100f;

		// Token: 0x040027F6 RID: 10230
		public float density = 100f;
	}
}
