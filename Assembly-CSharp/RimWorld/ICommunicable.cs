using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016AB RID: 5803
	public interface ICommunicable
	{
		// Token: 0x06007F16 RID: 32534
		string GetCallLabel();

		// Token: 0x06007F17 RID: 32535
		string GetInfoText();

		// Token: 0x06007F18 RID: 32536
		void TryOpenComms(Pawn negotiator);

		// Token: 0x06007F19 RID: 32537
		Faction GetFaction();

		// Token: 0x06007F1A RID: 32538
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
