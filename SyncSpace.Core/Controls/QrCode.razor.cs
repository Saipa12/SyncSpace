namespace SyncSpace.Core.Controls;

public partial class QrCode : Component
{
	public QrCode() : base("qrCode")
	{
	}

	public void SetArgs(int width1, int height1, string data1) => Args = new object[] { width1, height1, data1 };
}