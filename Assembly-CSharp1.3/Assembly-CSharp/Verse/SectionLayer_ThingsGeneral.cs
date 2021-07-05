using System;

namespace Verse
{
	// Token: 0x020001E1 RID: 481
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x0004D750 File Offset: 0x0004B950
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0004D768 File Offset: 0x0004B968
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception printing ",
					t,
					" at ",
					t.Position,
					": ",
					ex.ToString()
				}));
			}
		}
	}
}
