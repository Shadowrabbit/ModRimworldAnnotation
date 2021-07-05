using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E57 RID: 3671
	public class Need_Sadism : Need
	{
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060054F7 RID: 21751 RVA: 0x001CC89E File Offset: 0x001CAA9E
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060054F8 RID: 21752 RVA: 0x001CC8AD File Offset: 0x001CAAAD
		public bool IsCritical
		{
			get
			{
				return this.CurLevel < 0.1f;
			}
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x001CAD4D File Offset: 0x001C8F4D
		public Need_Sadism(Pawn newPawn) : base(newPawn)
		{
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x001CC8BC File Offset: 0x001CAABC
		public override void NeedInterval()
		{
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.def.fallPerDay * 0.0025f;
		}
	}
}
