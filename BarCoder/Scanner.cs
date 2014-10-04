using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarCoder.Properties;

namespace BarCoder
{
    public partial class Scanner : Form
    {
        public Scanner()
        {
            InitializeComponent();
        }

        private void Scanner_DragDrop(object sender, DragEventArgs e)
        {
            //Console.WriteLine(11);
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            var splitorIndex = path.LastIndexOf('\\');
            var fileName = path.Substring(splitorIndex + 1);
            //var directoryName = path.Substring(0, splitorIndex);
            try
            {
                if (path.EndsWith("xls") || path.EndsWith("xlsx"))
                {
                    Data.Instance().LoadExcel(path);
                    label.Text = Resources.Scanner_Scanner_DragDrop_excelLoadSucess + fileName;
                    Code_input.Enabled = true;
                }
                else if (path.EndsWith(".log"))
                {
                    Data.Instance().LoadData(path);
                    label.Text = Resources.Scanner_Scanner_DragDrop_externalLoadSucess + fileName;
                }
                else
                {
                    MessageBox.Show(Resources.Scanner_Scanner_DragDrop_fileTypeError);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


        }
        private void Scanner_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None;
        }

        private void Code_input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Enter) return;

            var text = Code_input.Text.Replace("\n", "").Replace("\r", "").Trim();
            Code_input.Text = "";

            var ret =  Data.Instance().Check(text);
            passLable.BackColor = !ret.IsSuccess ? Color.Red : SystemColors.Window;
            passLable.Text = ret.ErrorMessage;

            var record = new ListViewItem(Data.Instance().GetId(text)+"");//这个是第一行第一列
            record.SubItems.Add(ret.IsSuccess + "");
            record.SubItems.Add(text);
            record.SubItems.Add(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            listView2.Items.Add(record);

            listView1.Items.Clear();
            foreach (var pair in ret.UserInfo)
            {
                var tmprecord = new ListViewItem(pair.Key + "");//这个是第一行第一列
                //record.SubItems.Add(pair.Key+"");
                tmprecord.SubItems.Add(pair.Value + "");
                listView1.Items.Add(tmprecord);
            }

            label.Text = ret.Entered + @"/" + ret.Total;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var text = listView2.FocusedItem.SubItems[2].Text;
                var ret = Data.Instance().Check(text);
                listView1.Items.Clear();
                foreach (var pair in ret.UserInfo)
                {
                    var tmprecord = new ListViewItem(pair.Key + "");//这个是第一行第一列
                    //record.SubItems.Add(pair.Key+"");
                    tmprecord.SubItems.Add(pair.Value + "");
                    listView1.Items.Add(tmprecord);
                }
            }
            catch (Exception exception)
            {
               Console.WriteLine(exception.Message); 
            }
        }
    }
}
