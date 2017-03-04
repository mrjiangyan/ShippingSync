using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Dal;

namespace ShppingSync
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTryConnection_Click(object sender, EventArgs e)
        {
            string message;
            if(string.IsNullOrEmpty(txtConnection.Text))
            {
                MessageBox.Show("请输入数据库连接字符串");
                txtConnection.Focus();
                return;
            }
            if(ConnectionFactory.TryConnection(txtConnection.Text,out message))
            {
                MessageBox.Show("数据库连接成功");
            }
            else
            {
                MessageBox.Show("数据库连接失败，错误原因:" + message);
            }
        }

        /// <summary>
        /// 切换选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabPage=tabControl1.TabPages[tabControl1.SelectedIndex];
            if(tabPage == tabConnection && string.IsNullOrEmpty(txtConnection.Text))
            {
                txtConnection.Text = ConnectionFactory.ConnectionString;
            }
        }

        private void btnSaveConnection_Click(object sender, EventArgs e)
        {
            ConnectionFactory.ConnectionString = txtConnection.Text;
        }
    }
}
