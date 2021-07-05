using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001071 RID: 4209
	public interface ICommunicable
	{
		// Token: 0x060063E6 RID: 25574
		string GetCallLabel();

		// Token: 0x060063E7 RID: 25575
		string GetInfoText();

		// Token: 0x060063E8 RID: 25576
		void TryOpenComms(Pawn negotiator);

		// Token: 0x060063E9 RID: 25577
		Faction GetFaction();

		// Token: 0x060063EA RID: 25578
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
