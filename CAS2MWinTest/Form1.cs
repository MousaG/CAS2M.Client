using CAS2MClientDataMan.DataMan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAS2MWinTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private async Task Run()
        {
            try
            {
                await new DataSender().FetchAndSendData(dtFromDate.Value, dtToDate.Value, (CAS2MClientDataMan.Enums.EntityType)Convert.ToInt32(comboBox1.Text), new Guid(txToken.Text), txTask.Text, new Uri(txUrl.Text));
            }
            catch(Exception ex)
            {
                EventManager.Inst.WriteError("run", ex, 441);
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Run();
        }
    }
}
