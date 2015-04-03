using System;
using System.Windows.Forms;


namespace comcash
{
	partial class TestData
	{
		public bool messBox (string mess)
		{
			DialogResult result = MessageBox.Show (mess, "POS Testing", MessageBoxButtons.OKCancel);
			if (result == DialogResult.OK)
				return true;
			if (result == DialogResult.Cancel)
				return false;
			return false;
		}
	}
}
