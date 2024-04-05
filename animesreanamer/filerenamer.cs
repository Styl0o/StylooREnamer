using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using MaterialSkin;
using MaterialSkin.Controls;



namespace animesreanamer
{
    public partial class filerenamer : MaterialSkin.Controls.MaterialForm
    {
        public List<string> fichie = new List<string>();
        public List<string> fichie2 = new List<string>();
        public List<string> fichiersvoila = new List<string>();
        public string nomanime = "name of the show season 1 episode ";
        public int episodefirst = 1;
        public string savedirectory = "";
        public List<containerdevideos> containerdevid = new List<containerdevideos>();
        public string secondchoicetext = "name of the show season 1 episode *";
        public bool firstcheck = true;
        public bool secondcheck = false;
        public int firstepisodereal = 1;
        public ListBox[] leslistesboxes = new ListBox[4];
        public filerenamer()
        {
            TManager.Theme = MaterialSkinManager.Themes.DARK;

            InitializeComponent();

            leslistesboxes[0] = listBox1;
            leslistesboxes[1] = listBox3;
            leslistesboxes[2] = listBox4;
            leslistesboxes[3] = listBox5;


            listBox1.HorizontalScrollbar = true;
            listBox1.IntegralHeight = true;
            listBox2.HorizontalScrollbar = true;
            listBox2.IntegralHeight = true;
            listBox3.HorizontalScrollbar = true;
            listBox3.IntegralHeight = true;
            listBox4.HorizontalScrollbar = true;
            listBox4.IntegralHeight = true;
            listBox5.HorizontalScrollbar = true;
            listBox5.IntegralHeight = true;

            materialCheckbox1.Checked = true;

            KeyPreview = true;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;


        }

        MaterialSkinManager TManager = MaterialSkinManager.Instance;
        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch1.Checked)
            {
                TManager.Theme = MaterialSkinManager.Themes.LIGHT;
                materialSwitch1.Text = "Light mode";
            }
            else
            {
                TManager.Theme = MaterialSkinManager.Themes.DARK;
                materialSwitch1.Text = "Dark mode";
                
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    ajouterlesfichiersalaliste(file);

                }

            }
        }


        public void mettrelesconteneurs(string pathe)
        {
            containerdevideos tempcontainer = new containerdevideos() ;
            tempcontainer.path = pathe;
            tempcontainer.nom = Path.GetFileName(pathe);
            FileInfo fileInfo = new FileInfo(pathe);
            long taile = fileInfo.Length;
            tempcontainer.taillechiffre = taile;
            var datee = fileInfo.CreationTime;
            tempcontainer.taille = (taile/ 1000) + "mb";
            tempcontainer.date = "" + datee;
            
            textBox5.Text = tempcontainer.path;
            textBox6.Text = tempcontainer.nom;
            textBox7.Text = tempcontainer.taille;
            textBox8.Text = tempcontainer.date;

            containerdevid.Add(tempcontainer);
            // MessageBox.Show("" + containerdevid[0].taillechiffre);
        }
        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualiserleslistesboxes(listBox1.SelectedIndex);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualiserleslistesboxes(listBox3.SelectedIndex);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualiserleslistesboxes(listBox4.SelectedIndex);
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualiserleslistesboxes(listBox5.SelectedIndex);
        }
        public void actualiserleslistesboxes(int selectedindex)
        {

            foreach (ListBox items in leslistesboxes)
            {
                
                items.SelectedIndex = selectedindex;
                

            }
        }



        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {

            if (listBox1.SelectedItem == null 
                && e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                ajouterlesfichiersalaliste(file);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            

        }
        public void ajouterlesfichiersalaliste(string file)
        {
            if(!listBox1.Items.Contains(file))
            {
                fichie.Add(file);
                mettrelesconteneurs(file);
                listBox1.Items.Add(file);
                listBox2.Items.Add(lesnoms());
                moveitemslesnoms();
                mettrelesconteneursdansleslistes();

            }

            else
            {
                if (MessageBox.Show("There is already the file in the list you wish to add it anyway ?"
                    , "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fichie.Add(file);
                    mettrelesconteneurs(file);
                    listBox1.Items.Add(file);
                    listBox2.Items.Add(lesnoms());
                    moveitemslesnoms();
                    mettrelesconteneursdansleslistes();
                }
                else
                {
                    return;
                }
            }
            
        }
        
        public void mettrelesconteneursdansleslistes()
        {
            foreach(containerdevideos item in containerdevid)
            {
                if(!listBox3.Items.Contains(item.nom)) listBox3.Items.Add(item.nom);
                /*if (!listBox4.Items.Contains(item.date)) listBox4.Items.Add(item.date);
                if (!listBox5.Items.Contains(item.taille)) listBox5.Items.Add(item.taille); */
            }
        }
        public void lesnomsplusclear()
        {
            listBox2.Items.Clear();
            lesnomsajouter();
        }
        public string lesnoms()
        {
            //extentiontemp = Path.GetExtension(extentiontemp);
            string nom = "";
            if (firstcheck == true && secondcheck == false)
            {
                if (secondchoicetext.Contains("*"))
                {
                    List<char> chartemp = new List<char>();
                    foreach (char c in secondchoicetext)
                    {
                        chartemp.Add(c);
                    }
                    int indexchar = chartemp.IndexOf('*');
                    chartemp.RemoveAt(indexchar);

                    string stringtemp = "" + (listBox1.Items.Count - 1 + firstepisodereal);
                    foreach (char c in stringtemp)
                    {
                        int currentchar = stringtemp.IndexOf(c);
                        chartemp.Insert(indexchar + currentchar, c);
                    }
                    string secondchoicetexttemp = "";
                    secondchoicetexttemp = string.Join("", chartemp);

                    nom = secondchoicetexttemp ;

                }
                
            }
            else if (firstcheck == false && secondcheck == true)
            {
                nom = nomanime + (episodefirst + listBox1.Items.Count - 1);
            }
            return nom;
        }
        public void lesnomsajouter()
        {
            for (int i = 0; i < listBox1.Items.Count ; i++)
            {
                string nom = "";
                if (firstcheck == true && secondcheck == false)
                {
                    if (secondchoicetext.Contains("*"))
                    {
                        List<char> chartemp = new List<char>();
                        foreach (char c in secondchoicetext)
                        {
                            chartemp.Add(c);
                        }
                        int indexchar = chartemp.IndexOf('*');
                        chartemp.RemoveAt(indexchar);
                                 
                        string stringtemp = "" + (i + firstepisodereal);


                        foreach (char c in stringtemp)
                        {
                            int currentchar = stringtemp.IndexOf(c);
                            chartemp.Insert(indexchar + currentchar, c);
                        }
                        string secondchoicetexttemp = "";
                        secondchoicetexttemp = string.Join("", chartemp);

                        nom = secondchoicetexttemp;

                    }

                }
                else if (firstcheck == false && secondcheck == true)
                {
                    nom = nomanime + (episodefirst + i);
                }

                listBox2.Items.Add(nom);
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

            /* 
             if(listBox1.SelectedItem != null && listBox2.SelectedItem == null)
            {
                if (listBox1.SelectedItem != null)
                {
                    if (savedirectory == "" || textBox1 == null) MessageBox.Show
                            ("please select a save directory");
                    else
                    {
                        listBox2.Items.Add(listBox1.SelectedItem);
                        listBox1.Items.Remove(listBox1.SelectedItem);
                    }

                }
                else if (listBox1.SelectedItem == null) MessageBox.Show
                        ("please select which épisode you want to transter");

            }


            else if (listBox1.SelectedItem == null && listBox2.SelectedItem != null)
            {
                if (listBox2.SelectedItem != null)
                {
                    listBox1.Items.Add(listBox2.SelectedItem);
                    listBox2.Items.Remove(listBox2.SelectedItem);

                }
                else if (listBox2.SelectedItem == null) MessageBox.Show
                            ("please select which épisode you want to back");
            }
            else
            {
                MessageBox.Show("Please select which item you want to transfer or back");
            }*/
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = savedirectory;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            nomanime = textBox2.Text;
            lesnomsplusclear();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string itemtomove = listBox1.GetItemText(listBox1.SelectedItem);
            moveitems(-1, itemtomove);
            moveitemslesnoms();
            listBox3.SelectedIndex = listBox1.SelectedIndex;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string itemtomove = listBox1.GetItemText(listBox1.SelectedItem);
            moveitems(1, itemtomove);
            moveitemslesnoms();
            listBox3.SelectedIndex = listBox1.SelectedIndex;
        }

        


        public void moveitems(int plusunenbasmoinsunenbas ,string itemtomove)
        {

            if (listBox1.SelectedItems != null)
            {

                if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex <= listBox1.Items.Count - 1)
                {
                    int nouveauindex = listBox1.SelectedIndex + plusunenbasmoinsunenbas;
                    if (nouveauindex > listBox1.Items.Count - 1 || nouveauindex == -1) 
                    return;
                    
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);     
                    listBox1.Items.Insert(nouveauindex, itemtomove);
                    listBox1.SelectedIndex = nouveauindex;

                    containerdevideos tempcontain = new containerdevideos();
                    tempcontain = containeractualiserunseul(itemtomove);

                    containerdevid.RemoveAt(listBox1.SelectedIndex);
                    containerdevid.Insert(nouveauindex, tempcontain );
                    

                }
            }
            else if (listBox1.SelectedIndex == 0) listBox1.SelectedIndex = -1;
            else if (listBox1.SelectedItem == null) MessageBox.Show("select an item please my friend");
        }
        public void moveitemslesnoms()
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            
            foreach (string item in listBox1.Items)
            {
                FileInfo fileInfo = new FileInfo(item);
                long taille = fileInfo.Length;
                
                var datee = fileInfo.CreationTime;
                var t = (taille / 1000) + "mb";
                var d = "" + datee;

                listBox3.Items.Add(Path.GetFileName(item));
                
                listBox4.Items.Add(t);
                //Console.WriteLine(containerdevid[listBox1.Items.IndexOf(item)].date);
                listBox5.Items.Add(d);
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            this.KeyPreview = true;

            if (e.KeyCode == Keys.Z &&  listBox1.SelectedItem != null)
            {
                string itemtomove = listBox1.GetItemText(listBox1.SelectedItem);
                moveitems(-1, itemtomove);
            }
            if (e.KeyCode == Keys.S && listBox1.SelectedItem != null)
            {
                string itemtomove = listBox1.GetItemText(listBox1.SelectedItem);
                moveitems(1, itemtomove);
            }

            if (e.KeyCode == Keys.Delete && listBox1.SelectedItem != null)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                listBox1.SelectedIndex = -1;
            }
        }

   

        
        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox2.Checked == false) materialCheckbox1.Checked = true;

            firstcheck = true;
            secondcheck = false;
            materialCheckbox2.Checked = false;
            
            lesnomsplusclear();
        }

        private void materialCheckbox2_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox1.Checked == false) materialCheckbox2.Checked = true;
            firstcheck = false;
            secondcheck = true;
            materialCheckbox1.Checked = false;
            lesnomsplusclear();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            secondchoicetext = textBox9.Text;
            lesnomsplusclear();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void filerenamer_Load(object sender, EventArgs e)
        {

        }

        private void materialListBox1_SelectedIndexChanged(object sender, MaterialListBoxItem selectedItem)
        {

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dgl = new CommonOpenFileDialog();
            dgl.IsFolderPicker = true;
            if (dgl.ShowDialog() == CommonFileDialogResult.Ok)
            {
                savedirectory = dgl.FileName;
                textBox1.Text = savedirectory;
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items == null && fichiersvoila == null)
                MessageBox.Show("do something bruh ");
            else if (savedirectory == "") MessageBox.Show("choose a save directory please");

            else
            {
                foreach (string item in listBox1.Items)
                {
                    // fichiersvoila.Add(item);
                }
                foreach (string item in listBox1.Items)
                {
                    int currentindex = listBox1.Items.IndexOf(item);
                    string extension = Path.GetExtension(item);
                    string labastemp = "";
                    if (firstcheck == false && secondcheck == true)
                    {
                        labastemp = savedirectory + "/" + nomanime + " "
                      + (episodefirst + currentindex) + extension;

                    }
                    if (firstcheck == true && secondcheck == false)
                    {
                        if (secondchoicetext.Contains("*"))
                        {
                            List<char> chartemp = new List<char>();
                            foreach (char c in secondchoicetext)
                            {
                                chartemp.Add(c);
                            }
                            int indexchar = chartemp.IndexOf('*');


                            chartemp.RemoveAt(indexchar);

                            string stringtemp = "" + (currentindex + firstepisodereal);
                            foreach (char c in stringtemp)
                            {
                                int currentchar = stringtemp.IndexOf(c);
                                chartemp.Insert(indexchar + currentchar, c);
                            }
                            string secondchoicetexttemp = "";
                            secondchoicetexttemp = string.Join("", chartemp);




                            labastemp = savedirectory + "/" + secondchoicetexttemp + extension;



                        }
                    }

                    if (File.Exists(labastemp))
                    {
                        MessageBox.Show("warning this file exist already please choose another name " + labastemp);
                        return;
                    }
                    else if (!File.Exists(labastemp)) File.Copy(item, labastemp);
                }


            }





        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            firstepisodereal = (int) numericUpDown1.Value;
            lesnomsplusclear();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            episodefirst = (int)numericUpDown2.Value;
            lesnomsplusclear();
        }

        public containerdevideos containeractualiserunseul(string pathe)
        {
            containerdevideos tempcontainer = new containerdevideos();
            tempcontainer.path = pathe;
            tempcontainer.nom = Path.GetFileName(pathe);
            FileInfo fileInfo = new FileInfo(pathe);
            long taile = fileInfo.Length;
            tempcontainer.taillechiffre = taile;
            var datee = fileInfo.CreationTime;
            tempcontainer.taille = (taile / 1000) + "mb";
            tempcontainer.date = "" + datee;
            return tempcontainer;
        }

        private void restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void materialComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public class containerdevideos
    {
        public string nom;
        public string taille;
        public string date;
        public string path;
        public long taillechiffre;

        /*public containerdevideos(string nom, string taille,
            string date, string path, long taillechiffre)
        {
            this.nom = nom;
            this.taille= taille;
            this.date = date;
            this.path = path;
            this.taillechiffre = taillechiffre;
        }*/
    }
    
}
