using System;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x0600116A RID: 4458 RVA: 0x00012B1D File Offset: 0x00010D1D
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000C22E8 File Offset: 0x000C04E8
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
				}), false);
			}
		}
	}
}
