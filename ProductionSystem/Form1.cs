using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionSystem
{
    public partial class Form1 : Form
    {
        private List<Fact> all_facts = new List<Fact>();

        private List<Fact> choosen_facts = new List<Fact>();

        private List<Fact> all_notices = new List<Fact>();

        public Form1()
        {
            InitializeComponent();
        }

        private void get_facts(string filename)
        {
            List<string> lines = System.IO.File.ReadLines(filename, Encoding.GetEncoding(1251)).ToList();

            int cur_ind = 0;
            while(cur_ind < lines.Count && lines[cur_ind] != "" && lines[cur_ind][0] == 'f')
            {
                string cur = lines[cur_ind];
                int sep = cur.IndexOf(':');
                Fact f = new Fact( cur.Substring(0, sep - 1), cur.Substring(sep + 2));
                all_facts.Add(f);
                cur_ind++;
            }

            cur_ind++;
            while (cur_ind < lines.Count && lines[cur_ind] != "" && lines[cur_ind][0] == 'n')
            {
                string cur = lines[cur_ind];
                int sep = cur.IndexOf(':');
                Fact f = new Fact(cur.Substring(0, sep - 1), cur.Substring(sep + 2));
                all_notices.Add(f);
                cur_ind++;
            }


            
        }

        private void fill_checked_list()
        {
            foreach (Fact f in all_facts)
            {
                checkedListBox1.Items.Add(f.Value);
            }
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (!System.IO.File.Exists(ofd.FileName))
            {
                return;
            }

            get_facts(ofd.FileName);
            fill_checked_list();
        }

        
    }
}
