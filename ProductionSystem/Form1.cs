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
        //All existing facts
        private Dictionary<string, Fact> all_facts = new Dictionary<string, Fact>();

        //Facts that have been selected by the user
        private Dictionary<string, Fact> choosen_facts = new Dictionary<string, Fact>();

        //All existing notices
        private Dictionary<string, Fact> all_notices = new Dictionary<string, Fact>();

        //All existing rules
        private List<Rule> rules = new List<Rule>();

        //All existing terminal facts
        private Dictionary<string,Terminal> terminals = new Dictionary<string, Terminal>();

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
                all_facts[f.Id] = f;
                cur_ind++;
            }

            cur_ind++;
            while (cur_ind < lines.Count && lines[cur_ind] != "" && lines[cur_ind][0] == 'n')
            {
                string cur = lines[cur_ind];
                int sep = cur.IndexOf(':');
                Fact f = new Fact(cur.Substring(0, sep - 1), cur.Substring(sep + 2));
                all_notices[f.Id] = f;
                cur_ind++;
            }

            cur_ind++;
            while (cur_ind < lines.Count && lines[cur_ind] != "" && lines[cur_ind][0] == 't')
            {
                string cur = lines[cur_ind];
                int sep1 = cur.IndexOf(':');
                int sep2 = cur.IndexOf(';');
                Terminal t = new Terminal(cur.Substring(0, sep1 - 1), cur.Substring(sep1 + 2, sep2 - sep1 - 3), cur.Substring(sep2 + 2));
                terminals[t.Id] = t;
                cur_ind++;
            }

            cur_ind++;
            while (cur_ind < lines.Count && lines[cur_ind] != "" && lines[cur_ind][0] == 'r')
            {
                string cur = lines[cur_ind];
                int sep1 = cur.IndexOf(':');
                int sep2 = cur.IndexOf('-');

                string id = cur.Substring(0, sep1 - 1);
                string conditions = cur.Substring(sep1 + 2, sep2 - sep1 - 3);
                string conclusion = cur.Substring(sep2 + 3);
                List<string> cond = conditions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Rule r = new Rule(id,cond,conclusion);
                rules.Add(r);
                cur_ind++;
            }


        }

        private void fill_checked_list()
        {
            foreach (var f in all_facts)
            {
                checkedListBox1.Items.Add(f.Value.Value);
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

        

        private List<Terminal> forward_search()
        {
            List<string> id_choosen = new List<string>(choosen_facts.Keys);
            int step = 0;

            List<Rule> rules_to_use = rules.Where(r => r.Conditions.TrueForAll(s => id_choosen.Contains(s))).ToList();
            List<Rule> used_rules = new List<Rule>();
            List<Terminal> res = new List<Terminal>();
            while (rules_to_use.Count != 0)
            {
                step++;
                richTextBox1.Text += "Шаг #" + step + "\n";
                richTextBox1.Text += "Полученные факты: \n";

                foreach(Rule r in rules_to_use)
                {
                    if (!id_choosen.Contains(r.Conclusion))
                    {
                        id_choosen.Add(r.Conclusion);
                        if (r.Conclusion.Contains("f"))
                        {
                            richTextBox1.Text += r.Conclusion + " : " + all_facts[r.Conclusion].Value + "\n";
                        }
                        else if (r.Conclusion.Contains("n"))
                        {
                            richTextBox1.Text += r.Conclusion + " : " + all_notices[r.Conclusion].Value + "\n";
                        }
                        else
                        {
                            richTextBox1.Text += r.Conclusion + " : " + terminals[r.Conclusion].Value + "\n";
                            res.Add(terminals[r.Conclusion]);
                        }
                    }
                    used_rules.Add(r);
                }

                List<Rule> new_rules = rules.Where(r => r.Conditions.TrueForAll(s => id_choosen.Contains(s))).ToList();
                rules_to_use.Clear();
                foreach (Rule r in new_rules)
                {
                    if (!used_rules.Contains(r))
                    {
                        rules_to_use.Add(r);
                    }
                }
            }
            return res;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            foreach(string s in checkedListBox1.CheckedItems)
            {
                foreach(var f in all_facts)
                {
                    if (f.Value.Value == s)
                    {
                        choosen_facts[f.Key] = f.Value;
                    }
                }
            }

            List<Terminal> res = forward_search();
            pictureBox1.Image = new Bitmap(res.First().Img);
        }


    }
}
