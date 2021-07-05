using System;

namespace Verse.Sound
{
	// Token: 0x0200055B RID: 1371
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x000F715F File Offset: 0x000F535F
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000F7168 File Offset: 0x000F5368
		public override void SetOn(Sample sample, float value)
		{
			float num;
			if (this.pitchType == PitchParamType.Multiply)
			{
				num = value;
			}
			else
			{
				num = (float)Math.Pow(1.05946, (double)value);
			}
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}

		// Token: 0x0400191C RID: 6428
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType;
	}
}
