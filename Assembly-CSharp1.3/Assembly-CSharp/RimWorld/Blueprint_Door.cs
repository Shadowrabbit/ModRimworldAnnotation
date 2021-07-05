using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001041 RID: 4161
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06006259 RID: 25177 RVA: 0x002159C8 File Offset: 0x00213BC8
		public override Graphic Graphic
		{
			get
			{
				return base.DefaultGraphic;
			}
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x002159D0 File Offset: 0x00213BD0
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
