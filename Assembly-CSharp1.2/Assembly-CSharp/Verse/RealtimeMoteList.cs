using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F5 RID: 1269
	public class RealtimeMoteList
	{
		// Token: 0x06001F92 RID: 8082 RVA: 0x0001BCC1 File Offset: 0x00019EC1
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0001BCCE File Offset: 0x00019ECE
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0001BCDC File Offset: 0x00019EDC
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x001006EC File Offset: 0x000FE8EC
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x0400162D RID: 5677
		public List<Mote> allMotes = new List<Mote>();
	}
}
