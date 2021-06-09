using System;

namespace RimWorld
{
	// Token: 0x02001669 RID: 5737
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x06007D0F RID: 32015 RVA: 0x0005401C File Offset: 0x0005221C
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
