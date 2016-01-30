using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Formok;

namespace FakPlusz
{
    public partial class MainAlap : Form
    {
        public FakUserInterface FakUserInterface;
        public string[] connstringek = null;
        public bool close = false;
        public Base AktivControl = null;
        public MainControlAlap MainControl;
        public string Alkalmazas = "";
        public string MainControlNev = "";
        public Bejelentkezo Bejelentkezo;
        public MainAlap()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hu-HU");
        }
        public virtual void Main_Load(object sender, EventArgs e)
        {
            this.Refresh();
            panel1.Visible = false;
            //            connstringek = GetConnStrings.GetConnectionStrings("Connection.txt", "BackupPath.txt");
            connstringek = GetConnStrings.GetConnectionStrings("Connection.txt");
            if (connstringek == null)
            {
                close = true;
                this.Close();
            }
            else

            {
                close = AlkalmazasBejelentkezes();
                if (close)
                {
                    this.Close();
                    return;
                }
                this.Refresh();
                FakUserInterface = new FakUserInterface(Alkalmazas, this, true, connstringek, null, -1);
                if (FakUserInterface.BajVan)
                {
                    close = true;
                    this.Close();
                    return;
                }
//                else
//                {
//                    FakUserInterface.OpenProgress();
                if (AlkalmazasMainControlIndit() || AktivControl == null)
                {
                    this.Close();
                    return;
                }
               AktivControl.Dock = DockStyle.Fill;
               panel1.Visible = true;
//                    AktivControl = new TervControl(FakUserInterface, panel1, this);
//                    FakUserInterface.CloseProgress();
//                    AktivControl.Dock = DockStyle.Fill;
//                    panel1.Visible = true;
//                }
            }
        }
        public virtual bool AlkalmazasBejelentkezes()
        {
            return true;
        }
        public virtual bool AlkalmazasMainControlIndit()
        {
            return true;
        }
        public virtual bool AlkalmazasKiegTolt(Ceginformaciok ceginfo)
        {
            return true;
        }
        public virtual void MainForm_Closeing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                FakPlusz.MessageBox.DialogResult valasz = FakUserInterface.MunkaVege(this);
                if (valasz == FakPlusz.MessageBox.DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}
