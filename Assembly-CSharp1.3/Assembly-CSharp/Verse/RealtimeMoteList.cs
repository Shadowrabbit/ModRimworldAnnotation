using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000365 RID: 869
	public class RealtimeMoteList
	{
		// Token: 0x06001899 RID: 6297 RVA: 0x000916B7 File Offset: 0x0008F8B7
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000916C4 File Offset: 0x0008F8C4
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000916D2 File Offset: 0x0008F8D2
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000916E4 File Offset: 0x0008F8E4
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x040010AF RID: 4271
		public List<Mote> allMotes = new List<Mote>();
	}
}
