using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FakPlusz
{
    /// <summary>
    /// Magyaritott MessageBox
    /// </summary>
    public static class MessageBox
    {
        /// <summary>
        /// Lehetseges Button-kombinaciok enum-ja
        /// </summary>
        public enum MessageBoxButtons 
        {
            /// <summary>
            /// Buttonok: OK 
            /// </summary>
            OK, 
            /// <summary>
            /// Buttonok: OK, Megsem
            /// </summary>
            OKMégsem,
            /// <summary>
            /// Buttonok: Igen, Nem
            /// </summary>
            IgenNem,
            /// <summary>
            /// Buttonok: Igen, Nem, Megsem
            /// </summary>
            IgenNemMégsem,
            /// <summary>
            /// Nincs Button
            /// </summary>
            None };
        /// <summary>
        /// Lehetseges valaszok enum-ja
        /// </summary>
        public enum DialogResult 
        {
            /// <summary>
            ///  Valasz: OK
            /// </summary>
            OK,
            /// <summary>
            /// Valasz: Megsem
            /// </summary>
            Mégsem,
            /// <summary>
            /// Valasz: Igen
            /// </summary>
            Igen,
            /// <summary>
            /// Valasz: Nem
            /// </summary>
            Nem,
            /// <summary>
            /// Nincs valasz
            /// </summary>
            None,
            /// <summary>
            /// Valasz: Cancel
            /// </summary>
            Cancel };
        /// <summary>
        /// A MessageBox Form-ja
        /// </summary>
        public static MessageAlap Showbox = null;
        /// <summary>
        /// Ok button
        /// </summary>
        private static Button ButtonOk = new Button();
        /// <summary>
        /// Megsem button
        /// </summary>
        private static Button ButtonMegsem = new Button();
        /// <summary>
        /// Igen button
        /// </summary>
        private static Button ButtonIgen = new Button();
        /// <summary>
        /// Nem button
        /// </summary>
        private static Button ButtonNem = new Button();
        /// <summary>
        /// Uzenet, nincs Button, nincs Form Text property
        /// </summary>
        /// <param name="text">
        /// Ez az uzenet
        /// </param>
        /// <returns>
        /// lenyegtelen
        /// </returns>
        public static DialogResult Show(string text)
        {
            return Show(text, "");
        }
         
        /// <summary>
        /// Uzenet, nincs Button, van Form Text property
        /// </summary>
        /// <param name="text">
        /// Ez az uzenet
        /// </param>
        /// <param name="nametext">
        /// Ez a Form Text property
        /// </param>
        /// <returns>
        /// lenyegtelen
        /// </returns>
        public static DialogResult Show(string text, string nametext)
        {
            return Show(text, nametext, MessageBoxButtons.None);
        }
        /// <summary>
        /// Valaszra varo uzenet
        /// </summary>
        /// <param name="text">
        /// Ez az uzenet
        /// </param>
        /// <param name="nametext">
        /// Ez a Form Text property
        /// </param>
        /// <param name="buttonsenum">
        /// A valaszt eldonto buttonok tombje (max 3)
        /// </param>
        /// <returns>
        /// a valasz DialogResult
        /// </returns>
        public static DialogResult Show(string text, string nametext, MessageBoxButtons buttonsenum)
        {
            ButtonOk.Text = "OK";
            ButtonOk.Size = new System.Drawing.Size(56, 23);
            ButtonIgen.Text = "Igen";
            ButtonIgen.Size = new System.Drawing.Size(56, 23);
            ButtonNem.Text = "Nem";
            ButtonNem.Size = new System.Drawing.Size(56, 23);
            ButtonMegsem.Text = "Mégsem";
            ButtonMegsem.Size = new System.Drawing.Size(56, 23);
            switch ((int)(MessageBoxButtons)buttonsenum)
            {
                case 0:

                    Showbox = new MessageAlap(text, nametext, new Button[] { ButtonOk });
                    break;
                case 1:
                    Showbox = new MessageAlap(text, nametext, new Button[] { ButtonOk, ButtonMegsem });
                    break;
                case 2:
                    Showbox = new MessageAlap(text, nametext, new Button[] { ButtonIgen, ButtonNem });
                    break;
                case 3:
                    Showbox = new MessageAlap(text, nametext, new Button[] { ButtonIgen, ButtonNem, ButtonMegsem });
                    break;
                case 4:
                    Showbox = new MessageAlap(text, nametext, null);
                    break;
            }
            System.Windows.Forms.DialogResult ret = Showbox.ShowDialog();
            switch (ret)
            {
                case System.Windows.Forms.DialogResult.Cancel:
                    return DialogResult.Cancel;
                case System.Windows.Forms.DialogResult.None:
                    return DialogResult.None;
                case System.Windows.Forms.DialogResult.No:
                    return DialogResult.Nem;
                case System.Windows.Forms.DialogResult.Yes:
                    return DialogResult.Igen;
                case System.Windows.Forms.DialogResult.OK:
                    return DialogResult.OK;
                default:
                    return DialogResult.Cancel;
            }
        }
        /// <summary>
        /// MessageBox zaras
        /// </summary>
        public static void Close()
        {
            Showbox.Close();
        }
    }
}
