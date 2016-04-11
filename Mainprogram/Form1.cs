using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace Mainprogram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "xml notes (*.xml)|*.xml";

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                pathfn.Text = "";
                pathfn.Text = @openFileDialog2.FileName;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "xml notes (*.xml)|*.xml";

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                pathon.Text = "";
                pathon.Text = @openFileDialog2.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path2ownNotes = pathon.Text;
            string path2foreignNotes = pathfn.Text;
            string path2saveNewNotes = TextBoxPathNewNote.Text;

            if ((path2ownNotes != "") && (path2foreignNotes != "") && (path2saveNewNotes != "")) {
                NotesComparer notesComparer = new NotesComparer();
                notesComparer.compareNotes(path2ownNotes, path2foreignNotes, path2saveNewNotes);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowseDialog1 = new FolderBrowserDialog();

            if (FolderBrowseDialog1.ShowDialog() == DialogResult.OK)
            {
                TextBoxPathNewNote.Text = "";
                TextBoxPathNewNote.Text = @FolderBrowseDialog1.SelectedPath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

            }
        }
    }
}
