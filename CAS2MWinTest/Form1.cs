using CAS2MClientDataMan.DataMan;
using CAS2MClientDataMan.Domain;
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
        private async void RunAsync()
        {
            try
            {
                var ds = new DataSender();
                ds.OnProgress += new EventHandler(ShowProgress);
                ds.OnProgress += new EventHandler(ShowError);
                await ds.FetchAndSendData(dtFromDate.Value, dtToDate.Value, (CAS2MClientDataMan.Enums.EntityType)Convert.ToInt32(comboBox1.Text), new Guid(txToken.Text), txTask.Text, new Uri(txUrl.Text));
            }
            catch(Exception ex)
            {
                EventManager.Inst.WriteError("run", ex, 441);
                MessageBox.Show(ex.Message);
            }
        }
        private TaskData SetTask()
        {
            try
            {
                var ds = new DataSender();
                ds.OnProgress += new EventHandler(ShowProgress);  
                return  ds.SetTask(new Uri(txUrl.Text), dtFromDate.Value, dtToDate.Value, (CAS2MClientDataMan.Enums.EntityType)Convert.ToInt32(comboBox1.Text));
            }
            catch (Exception ex)
            {
                EventManager.Inst.WriteError("run", ex, 441);
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RunAsync();
        }
        public void ShowProgress(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                label7.Text=sender.ToString();
            });
        }
        public void ShowError(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                label7.Text = sender.ToString();
                MessageBox.Show(sender.ToString());
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tdata = SetTask();

            if (tdata != null)
            {
                txTask.Text = tdata.taskid;
                txToken.Text = tdata.taskToken.ToString();
            }

        }
    }
}
