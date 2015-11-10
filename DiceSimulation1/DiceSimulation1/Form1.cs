using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiceSimulation1
{
    public partial class Form1 : Form
    {

        // Creating RollLists to capture dice rolls for comparison
        List<DiceRollList> RollList1 = new List<DiceRollList>();
        List<DiceRollList> RollList2 = new List<DiceRollList>();

        // Creating SumLists to capture sum of rolls for search
        List<int> SumList1 = new List<int>();
        List<int> SumList2 = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        //DiceRoll Method to roll dice
        private void DiceRoll()
        {
            //Clear list for new roll
            listView1.Items.Clear();

            //Setup random
            Random shooter = new Random();

            //For loop for first set of 100 rolls
            for (int i = 1; i <= 100; ++i)
            {
                String result;
                int die1, die2, die1Value, die2Value, total;

                //Roll both dice
                die1 = shooter.Next(6);
                die2 = shooter.Next(6);

                //Increase both dice by 1 to get dice value
                die1Value = die1 + 1;
                die2Value = die2 + 1;
                total = die1Value + die2Value;

                // Adds total to SumList for First set of rolls
                SumList1.Add(total);

                //Adds total to RollList1 for first set of rolls
                RollList1.Add(new DiceRollList { RollNum = i, Dice1 = die1Value, Dice2 = die2Value, DiceTotal = total });


                //Change image for each Die to represent the number rolled
                lbl_die1.ImageIndex = die1;
                lbl_die2.ImageIndex = die2;

                // Changes label7 text to roll number
                label7.Text = i.ToString();

                //if statement to determine if both dice were the same number
                if (die1Value == die2Value)
                {
                    result = "On roll number " + i.ToString() + " both dice rolled " + die1Value.ToString() + "'s";
                    string[] row = { i.ToString(), die1Value.ToString(), die2Value.ToString(), total.ToString(), result };
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                    //force application to update
                    Application.DoEvents();
                }
                else
                {

                    string[] row = { i.ToString(), die1Value.ToString(), die2Value.ToString(), total.ToString() };
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                    //force application to update
                    Application.DoEvents();
                }

                //Force thread to sleep to create a pause between each for loop
                System.Threading.Thread.Sleep(600);
            }

            // For loop for second set of 100 rolls
            for (int i = 1; i <= 100; ++i)
            {
                String result;
                int die1, die2, die1Value, die2Value, total;

                //Roll both dice
                die1 = shooter.Next(6);
                die2 = shooter.Next(6);

                //Increase both dice by 1 to get dice value
                die1Value = die1 + 1;
                die2Value = die2 + 1;
                total = die1Value + die2Value;

                //Adds total to RollList1 for first set of rolls
                RollList2.Add(new DiceRollList { RollNum = i, Dice1 = die1Value, Dice2 = die2Value, DiceTotal = total });

                // Adds total to SumList for First set of rolls
                SumList2.Add(total);



                //Change image for each Die to represent the number rolled
                lbl_die1.ImageIndex = die1;
                lbl_die2.ImageIndex = die2;

                // Changes label7 text to roll number
                label7.Text = i.ToString();

                //if statement to determine if both dice were the same number
                if (die1Value == die2Value)
                {
                    result = "On roll number " + i.ToString() + " both dice rolled " + die1Value.ToString() + "'s";
                    string[] row = { i.ToString(), die1Value.ToString(), die2Value.ToString(), total.ToString(), result };
                    var listViewItem = new ListViewItem(row);
                    listView2.Items.Add(listViewItem);
                    //force application to update
                    Application.DoEvents();
                }
                else
                {

                    string[] row = { i.ToString(), die1Value.ToString(), die2Value.ToString(), total.ToString() };
                    var listViewItem = new ListViewItem(row);
                    listView2.Items.Add(listViewItem);
                    //force application to update
                    Application.DoEvents();
                }

                //Force thread to sleep to create a pause between each for loop
                System.Threading.Thread.Sleep(600);
            }



        }

        //Set up method for query search of sum totals
        public void QuerySearch()
        {
            // selected item to string to determine which number was selected
            String numSelect = comboBox1.SelectedItem.ToString();
            int totCount = 0;
            
            //Query of first sumlist
            IEnumerable<int> queryTotal =
                 from int total in SumList1
                 where total == Int32.Parse(numSelect)
                 select total;
            var count = queryTotal.Count();

            //Query of second sumlist
            IEnumerable<int> queryTotal2 =
                 from int total in SumList2
                 where total == Int32.Parse(numSelect)
                 select total;
            var count2 = queryTotal2.Count();

            //Adding total of both queries to produce total results
            totCount = count + count2;

            //Set label 5 to show results
            label5.Text = "Total: " + numSelect + " Found Count: " + totCount.ToString();

        }





        //Create method to compare using IEnumerable.Except of both roll lists
        public void Compare()
        {

            IEnumerable<DiceRollList> except = RollList1.Except(RollList2);

            //Display results
            foreach (var rollList in except)
            {

                string[] row = { rollList.RollNum.ToString(), rollList.Dice1.ToString(), rollList.Dice2.ToString(), rollList.DiceTotal.ToString() };
                var listViewItem = new ListViewItem(row);
                listView3.Items.Add(listViewItem);
            }



        }


        //Create class DiceRollList in order to create lists of rolls
        public class DiceRollList : IEquatable<DiceRollList>
        {
            public int RollNum { get; set; }
            public int Dice1 { get; set; }
            public int Dice2 { get; set; }
            public int DiceTotal { get; set; }

            public bool Equals(DiceRollList other)
            {


                return Dice1 == other.Dice1 && Dice2 == other.Dice2 || Dice1 == other.Dice2 && Dice2 == other.Dice1;
            
            
            }

            public override int GetHashCode()
            {
                
                if (Dice1 > Dice2)
                    return 11 * Dice1 + Dice2;
                else
                    return 11 * Dice2 + Dice1;
            }


        }

        // Roll Dice Button
        private void button1_Click(object sender, EventArgs e)
        {
            DiceRoll();
        }

        //Clear Results Button
        private void button2_Click(object sender, EventArgs e)
        {
            //Clear list view without re-rolling
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            label5.Text = " ";
            label7.Text = " ";

            //Clearn Lists 
            RollList1.Clear();
            RollList2.Clear();
            SumList1.Clear();
            SumList2.Clear();

        }

        //Exit Button
        private void button3_Click(object sender, EventArgs e)
        {
            //alternate close application
            this.Close();
        }

        //Search Button
        private void button4_Click(object sender, EventArgs e)
        {
            QuerySearch();
        }

        //Show Results Button
        private void button5_Click(object sender, EventArgs e)
        {
            Compare();
        }
    }
}
