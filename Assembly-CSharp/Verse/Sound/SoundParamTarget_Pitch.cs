using System;

namespace Verse.Sound
{
	// Token: 0x02000930 RID: 2352
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x0002CAD9 File Offset: 0x0002ACD9
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x001683DC File Offset: 0x001665DC
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

		// Token: 0x04002819 RID: 10265
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType;
	}
}
