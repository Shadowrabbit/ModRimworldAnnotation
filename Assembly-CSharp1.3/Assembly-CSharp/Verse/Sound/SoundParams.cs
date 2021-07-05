using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200057A RID: 1402
	public class SoundParams
	{
		// Token: 0x17000826 RID: 2086
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

		// Token: 0x06002930 RID: 10544 RVA: 0x000F93E1 File Offset: 0x000F75E1
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x04001983 RID: 6531
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04001984 RID: 6532
		public SoundSizeAggregator sizeAggregator;
	}
}
