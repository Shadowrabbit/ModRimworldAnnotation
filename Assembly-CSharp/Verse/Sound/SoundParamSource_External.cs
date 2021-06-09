using System;

namespace Verse.Sound
{
	// Token: 0x02000922 RID: 2338
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x0002C926 File Offset: 0x0002AB26
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

		// Token: 0x060039C8 RID: 14792 RVA: 0x001682D0 File Offset: 0x001664D0
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		// Token: 0x04002809 RID: 10249
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x0400280A RID: 10250
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
