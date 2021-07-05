using System;

namespace Verse.Sound
{
	// Token: 0x0200054D RID: 1357
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000F6E9F File Offset: 0x000F509F
		public override string Label
		{
			get
			{
				if (this.inParamName == "")
				{
					return "Undefined external";
				}
				return this.inParamName;
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000F6EC0 File Offset: 0x000F50C0
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		// Token: 0x0400190C RID: 6412
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x0400190D RID: 6413
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
