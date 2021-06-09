using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000955 RID: 2389
	public class SoundParams
	{
		// Token: 0x17000968 RID: 2408
		public float this[string key]
		{
			get
			{
				return this.storedParams[key];
			}
			set
			{
				this.storedParams[key] = value;
			}
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x0002D113 File Offset: 0x0002B313
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x0400288B RID: 10379
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x0400288C RID: 10380
		public SoundSizeAggregator sizeAggregator;
	}
}
