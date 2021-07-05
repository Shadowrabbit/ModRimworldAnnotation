using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8D RID: 3981
	public class SerializablePawnList : IExposable
	{
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06005E4C RID: 24140 RVA: 0x00205E07 File Offset: 0x00204007
		public List<Pawn> Pawns
		{
			get
			{
				return this.pawns;
			}
		}

		// Token: 0x06005E4D RID: 24141 RVA: 0x000033AC File Offset: 0x000015AC
		public SerializablePawnList()
		{
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x00205E0F File Offset: 0x0020400F
		public SerializablePawnList(List<Pawn> pawns)
		{
			this.pawns = pawns;
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x00205E1E File Offset: 0x0020401E
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400366F RID: 13935
		private List<Pawn> pawns;
	}
}
