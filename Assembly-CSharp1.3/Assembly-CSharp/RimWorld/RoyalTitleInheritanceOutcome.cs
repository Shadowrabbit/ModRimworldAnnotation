using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001521 RID: 5409
	public struct RoyalTitleInheritanceOutcome
	{
		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x060080AE RID: 32942 RVA: 0x002D97FB File Offset: 0x002D79FB
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x060080AF RID: 32943 RVA: 0x002D9806 File Offset: 0x002D7A06
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		// Token: 0x04005024 RID: 20516
		public Pawn heir;

		// Token: 0x04005025 RID: 20517
		public RoyalTitleDef heirCurrentTitle;

		// Token: 0x04005026 RID: 20518
		public bool heirTitleHigher;
	}
}
