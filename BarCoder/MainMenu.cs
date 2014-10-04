using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarCoder
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void StartChecker(object sender, EventArgs e)
        {
            new Scanner().ShowDialog();
        }

        private void GenerateCode(object sender, EventArgs e)
        {

        }

        private void OnLoad(object sender, EventArgs e)
        {

        }
    }
}
