using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml.Schema;

namespace L1_17
{
    public partial class Forma1 : System.Web.UI.Page
    {
        /// <summary>
        /// N(height) and M(length) of the tables
        /// </summary>
        private int N;
        private int M;
        
        /// <summary>
        /// cubes with a "checked" status
        /// </summary>
        public class Cube
        {
            public bool Checked { get; set; }
            public Cube()
            {
                Checked = false;
            }
            
        }
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }
        /// <summary>
        /// Resets the tables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            Label4.Visible = false;
        }
        /// <summary>
        /// creates 2 tables and finds the biggest patch of same color connected cubes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label3.Visible = false;
            if (Validation())
            {
                Label3.Visible = true;
                return;
            }
            

            N = int.Parse(TextBox1.Text);
            M = int.Parse(TextBox2.Text);

            Random random = new Random();

            List<List<Cube>> Cubes1 = new List<List<Cube>>();
            CreateTable(Table1, Cubes1, random);
            List<List<Cube>> Cubes2 = new List<List<Cube>>();
            CreateTable(Table2, Cubes2, random);

            
            int MaxI = -1;
            int MaxJ = -1;
            int MaxBlob = 0;


            Cube TopCube = new Cube();
            
            Func(true,Cubes1,Cubes2,Table1,Table2,ref MaxI,ref MaxJ,ref MaxBlob);

            //---------------------------------
            for (int ii = 0; ii < N; ii++)
            {
                for (int jj = 0; jj < M; jj++)
                {
                    Cubes1[ii][jj].Checked = false;
                    Cubes2[ii][jj].Checked = false;
                }
            }
            //----------------------------------

            bool StartSide = ConfirmaSide(MaxI, MaxJ, Cubes1, Cubes2, Table1, Table2);

            //---------------------------------
            for (int ii = 0; ii < N; ii++)
            {
                for (int jj = 0; jj < M; jj++)
                {
                    Cubes1[ii][jj].Checked = false;
                    Cubes2[ii][jj].Checked = false;
                }
            }
            //----------------------------------

            

            int ConnectorI = -1; int ConnectorJ = -1;

            Fill(MaxI,MaxJ,true,Cubes1,Cubes2,"",ref ConnectorI,ref ConnectorJ,ref StartSide);
            //---------------------------------
            int MaxTop = 0;
            int MaxBottom = 0;
            MaxBlob = 0;
            for (int ii = 0; ii < N; ii++)
            {
                for (int jj = 0; jj < M; jj++)
                {
                    if (Table1.Rows[ii].Cells[jj].Text != "aaa") MaxTop++;
                    if (Table2.Rows[ii].Cells[jj].Text != "aaa") MaxBottom++;
                }
            }
            MaxBlob = MaxTop + MaxBottom;
            //----------------------------------

            Label4.Visible = true;
            Label4.Text = "Didžiausią plotą sudaro viršuje "+ MaxTop + " ir apačioje "+MaxBottom+" langelių. Langelis: "+ (ConnectorI+1) + " eil., "+(ConnectorJ+1)+" st";
            
        }
        /// <summary>
        /// validates if the submited data is usable
        /// </summary>
        /// <returns></returns>
        public bool Validation()
        {
            if (Regex.IsMatch(TextBox1.Text.ToLower(), "[a-ząčęėįšųūž]") || Regex.IsMatch(TextBox2.Text.ToLower(), "[a-ząčęėįšųūž]"))
            {
                return true;
            }
            else if (int.Parse(TextBox1.Text) <= 0 || int.Parse(TextBox1.Text) > 20 || int.Parse(TextBox2.Text) <= 0 || int.Parse(TextBox2.Text) > 30) return true;

            return false;
        }
        /// <summary>
        /// Creates a table full of random color cubes
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cubes"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public Table CreateTable(Table table, List<List<Cube>> cubes, Random random)
        {


            for (int i = 0; i < N; i++)
            {
                TableRow row = new TableRow();
                List<Cube> list = new List<Cube>();
                for (int j = 0; j < M; j++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = "aaa";

                   
                    Cube cube = new Cube();

                    int num = random.Next(1, 4);
                    switch (num)
                    {
                        case 1:

                            cell.BackColor = Color.Red;
                            cell.ForeColor = Color.Red;

                            cube = new Cube();

                            break;
                        case 2:
                            cell.BackColor = Color.Yellow;
                            cell.ForeColor = Color.Yellow;

                            cube = new Cube();

                            break;
                        case 3:
                            cell.BackColor = Color.DarkOliveGreen;
                            cell.ForeColor = Color.DarkOliveGreen;


                            cube = new Cube();

                            break;
                    }
                    cell.BorderColor = Color.Black;
                    row.Cells.Add(cell);
                    list.Add(cube);
                }
                table.Rows.Add(row);
                cubes.Add(list);
            }
            return table;
        }
        /// <summary>
        /// The cycle in which the recursive function will run. Finds the biggest patch of cubes
        /// </summary>
        /// <param name="topside"></param>
        /// <param name="Cubes1"></param>
        /// <param name="Cubes2"></param>
        /// <param name="Table1"></param>
        /// <param name="Table2"></param>
        /// <param name="maxI"></param>
        /// <param name="maxJ"></param>
        /// <param name="MaxBlob"></param>
        public void Func(bool topside, List<List<Cube>> Cubes1, List<List<Cube>> Cubes2,Table Table1, Table Table2, ref int maxI, ref int maxJ, ref int MaxBlob)
        {
            
            maxI = -1;
            maxJ = -1;
            MaxBlob = 0;
            int CurrentBlob;

            string CurrentColor = "";
            for (int i = 0; i < N; i++ )
            {

                for(int j = 0; j < M; j++ )
                {
                    
                    Cubes1[i][j].Checked = true;
                    CurrentColor = Table1.Rows[i].Cells[j].BackColor.ToString();
                    CurrentBlob = 1;
                    if (topside)
                    {
                        
                        if (!Cubes1[i][j].Checked && CurrentColor == Table1.Rows[i].Cells[j].BackColor.ToString())
                        {

                            topside = true;
                            Cubes2[i][j].Checked = true;
                            CurrentBlob++;
                            if (CurrentBlob > MaxBlob)
                            {
                                MaxBlob = CurrentBlob;
                                maxI = i;
                                maxJ = j;
                                
                            }
                            Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                        }

                        if (!Cubes2[i][j].Checked && CurrentColor == Table2.Rows[i].Cells[j].BackColor.ToString())
                        {

                            topside = false;
                            Cubes1[i][j].Checked = true;
                            CurrentBlob++;
                            if (CurrentBlob > MaxBlob)
                            {
                                MaxBlob = CurrentBlob;
                                maxI = i;
                                maxJ = j;
                               
                            }
                            Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                        }

                        if (j < M-1)
                        {
                            if (!Cubes1[i][j + 1].Checked && CurrentColor == Table1.Rows[i].Cells[j + 1].BackColor.ToString())
                            {
                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i,j + 1,topside,Cubes1,Cubes2, CurrentColor, ref MaxBlob,ref CurrentBlob,ref maxI,ref maxJ);
                                


                            }   
                        }

                        if (i < N-1)
                        {
                            if (!Cubes1[i + 1][j].Checked && CurrentColor == Table1.Rows[i + 1].Cells[j].BackColor.ToString())
                            {
                               
                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                   
                                }
                                Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor,ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (j > 0)
                        {
                            if (!Cubes1[i][j - 1].Checked && CurrentColor == Table1.Rows[i].Cells[j - 1].BackColor.ToString())
                            {
                               
                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i > 0)
                        {
                            if (!Cubes1[i - 1][j].Checked && CurrentColor == Table1.Rows[i - 1].Cells[j].BackColor.ToString())
                            {
                                
                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        

                    }
                    if (!topside)
                    {
                        
                        if (j < M - 1)
                        {
                            if (!Cubes2[i][j + 1].Checked && CurrentColor == Table2.Rows[i].Cells[j + 1].BackColor.ToString())
                            {
                               
                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i < N - 1)
                        {
                            if (!Cubes2[i + 1][j].Checked && CurrentColor == Table2.Rows[i + 1].Cells[j].BackColor.ToString())
                            {
                               
                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                   
                                }
                                Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (j > 0)
                        {
                            if (!Cubes2[i][j - 1].Checked && CurrentColor == Table2.Rows[i].Cells[j - 1].BackColor.ToString())
                            {

                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i > 0)
                        {
                            if (!Cubes2[i - 1][j].Checked && CurrentColor == Table2.Rows[i - 1].Cells[j].BackColor.ToString())
                            {
                               
                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                               
                            }
                        }

                       

                    }
                    
                }

            }
            //starting from the other table incase some cubes were missed
            
            topside = false;
            for (int i = 0; i < N; i++)
            {

                for (int j = 0; j < M; j++)
                {

                    Cubes2[i][j].Checked = true;
                    CurrentColor = Table2.Rows[i].Cells[j].BackColor.ToString();
                    CurrentBlob = 1;
                    if (topside)
                    {
                        
                        if (!Cubes2[i][j].Checked && CurrentColor == Table2.Rows[i].Cells[j].BackColor.ToString())
                        {

                            topside = false;
                            Cubes1[i][j].Checked = true;
                            CurrentBlob++;
                            if (CurrentBlob > MaxBlob)
                            {
                                MaxBlob = CurrentBlob;
                                maxI = i;
                                maxJ = j;
                               
                            }
                            Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                        }

                        if (j < M - 1)
                        {
                            if (!Cubes1[i][j + 1].Checked && CurrentColor == Table1.Rows[i].Cells[j + 1].BackColor.ToString())
                            {
                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i < N - 1)
                        {
                            if (!Cubes1[i + 1][j].Checked && CurrentColor == Table1.Rows[i + 1].Cells[j].BackColor.ToString())
                            {

                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (j > 0)
                        {
                            if (!Cubes1[i][j - 1].Checked && CurrentColor == Table1.Rows[i].Cells[j - 1].BackColor.ToString())
                            {

                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                               
                            }
                        }

                        if (i > 0)
                        {
                            if (!Cubes1[i - 1][j].Checked && CurrentColor == Table1.Rows[i - 1].Cells[j].BackColor.ToString())
                            {

                                Cubes1[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                       

                    }
                    if (!topside)
                    {
                        
                        if (!Cubes1[i][j].Checked && CurrentColor == Table1.Rows[i].Cells[j].BackColor.ToString())
                        {

                            topside = true;
                            Cubes2[i][j].Checked = true;
                            CurrentBlob++;
                            if (CurrentBlob > MaxBlob)
                            {
                                MaxBlob = CurrentBlob;
                                maxI = i;
                                maxJ = j;
                                
                            }
                            Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                        }
                        if (j < M - 1)
                        {
                            if (!Cubes2[i][j + 1].Checked && CurrentColor == Table2.Rows[i].Cells[j + 1].BackColor.ToString())
                            {

                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i < N - 1)
                        {
                            if (!Cubes2[i + 1][j].Checked && CurrentColor == Table2.Rows[i + 1].Cells[j].BackColor.ToString())
                            {

                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (j > 0)
                        {
                            if (!Cubes2[i][j - 1].Checked && CurrentColor == Table2.Rows[i].Cells[j - 1].BackColor.ToString())
                            {

                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                        if (i > 0)
                        {
                            if (!Cubes2[i - 1][j].Checked && CurrentColor == Table2.Rows[i - 1].Cells[j].BackColor.ToString())
                            {

                                Cubes2[i][j].Checked = true;
                                CurrentBlob++;
                                if (CurrentBlob > MaxBlob)
                                {
                                    MaxBlob = CurrentBlob;
                                    maxI = i;
                                    maxJ = j;
                                    
                                }
                                Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                                
                            }
                        }

                       
                    }
                }
            }
        }
        /// <summary>
        /// Recursive funtion. Searches for cubes belonging to their own patch
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="topside"></param>
        /// <param name="Cubes1"></param>
        /// <param name="Cubes2"></param>
        /// <param name="CurrentColor"></param>
        /// <param name="MaxBlob"></param>
        /// <param name="CurrentBlob"></param>
        /// <param name="maxI"></param>
        /// <param name="maxJ"></param>
        public void Search(int i, int j, bool topside, List<List<Cube>> Cubes1, List<List<Cube>> Cubes2, string CurrentColor, ref int MaxBlob, ref int CurrentBlob, ref int maxI, ref int maxJ)
        {

            if (topside)
            {
                Cubes1[i][j].Checked = true;
                CurrentBlob++;
                if (CurrentBlob > MaxBlob)
                {
                    MaxBlob = CurrentBlob;
                    maxI = i;
                    maxJ = j;

                }
                if (!Cubes2[i][j].Checked && CurrentColor == Table2.Rows[i].Cells[j].BackColor.ToString() && Table2.Rows[i].Cells[j].BackColor.ToString() == Table1.Rows[i].Cells[j].BackColor.ToString())
                {
                    topside = false;
                   
                    Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                }

                if (j < M - 1)
                {
                    if (!Cubes1[i][j + 1].Checked && CurrentColor == Table1.Rows[i].Cells[j + 1].BackColor.ToString())
                    {

                        topside = true;
                        Search(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                if (i < N - 1)
                {
                    if (!Cubes1[i + 1][j].Checked && CurrentColor == Table1.Rows[i + 1].Cells[j].BackColor.ToString())
                    {

                        topside = true;
                        Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                if (j > 0)
                {
                    if (!Cubes1[i][j - 1].Checked && CurrentColor == Table1.Rows[i].Cells[j - 1].BackColor.ToString())
                    {

                        topside = true;
                        Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                if (i > 0)
                {
                    if (!Cubes1[i - 1][j].Checked && CurrentColor == Table1.Rows[i - 1].Cells[j].BackColor.ToString())
                    {

                        topside = true;
                        Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                

            }
            if (!topside)
            {
                Cubes2[i][j].Checked = true;
                CurrentBlob++;
                if (CurrentBlob > MaxBlob)
                {
                    MaxBlob = CurrentBlob;
                    maxI = i;
                    maxJ = j;

                }
                if (!Cubes1[i][j].Checked && CurrentColor == Table1.Rows[i].Cells[j].BackColor.ToString() && Table2.Rows[i].Cells[j].BackColor.ToString() == Table1.Rows[i].Cells[j].BackColor.ToString())
                {
                    topside = true;
                    
                    Search(i, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);

                }

                if (j < M - 1)
                {
                    if (!Cubes2[i][j + 1].Checked && CurrentColor == Table2.Rows[i].Cells[j + 1].BackColor.ToString())
                    {

                        topside = false;
                        Search(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                if (i < N - 1)
                {
                    if (!Cubes2[i + 1][j].Checked && CurrentColor == Table2.Rows[i + 1].Cells[j].BackColor.ToString())
                    {

                        topside = false;
                        Search(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                       
                    }
                }

                if (j > 0)
                {
                    if (!Cubes2[i][j - 1].Checked && CurrentColor == Table2.Rows[i].Cells[j - 1].BackColor.ToString())
                    {

                        topside = false;
                        Search(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                        
                    }
                }

                if (i > 0)
                {
                    if (!Cubes2[i - 1][j].Checked && CurrentColor == Table2.Rows[i - 1].Cells[j].BackColor.ToString())
                    {

                        topside = false;
                        Search(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref MaxBlob, ref CurrentBlob, ref maxI, ref maxJ);
                       
                    }
                }

            }

        }
        /// <summary>
        /// Recursive function. Fills in the the cubes of the biggest patch with "*"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="topside"></param>
        /// <param name="Cubes1"></param>
        /// <param name="Cubes2"></param>
        /// <param name="CurrentColor"></param>
        /// <param name="connectorI"></param>
        /// <param name="connectorJ"></param>
        /// <param name="StartSide"></param>
        public void Fill(int i, int j, bool topside, List<List<Cube>> Cubes1, List<List<Cube>> Cubes2, string CurrentColor, ref int connectorI, ref int connectorJ, ref bool StartSide)
        {
            if (StartSide && CurrentColor == "")
            {
                topside = true;
                Table1.Rows[i].Cells[j].Text = " * ";
                Table1.Rows[i].Cells[j].ForeColor = Color.Black;
                CurrentColor = Table1.Rows[i].Cells[j].BackColor.ToString();
            }
            else if (!StartSide && CurrentColor == "")
            {
                topside = false;
                Table2.Rows[i].Cells[j].Text = " * ";
                Table2.Rows[i].Cells[j].ForeColor = Color.Black;
                CurrentColor = Table2.Rows[i].Cells[j].BackColor.ToString();
            }



            if (topside)
            {
                Table1.Rows[i].Cells[j].Text = " * ";
                Table1.Rows[i].Cells[j].ForeColor = Color.Black;
                Cubes1[i][j].Checked = true;

                if (!Cubes2[i][j].Checked && CurrentColor == Table2.Rows[i].Cells[j].BackColor.ToString() && Table2.Rows[i].Cells[j].BackColor.ToString() == Table1.Rows[i].Cells[j].BackColor.ToString())
                {
                    topside = false;
                   
                    
                    connectorI = i;
                    connectorJ = j;
                    Fill(i, j, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);


                }

                if (j < M - 1)
                {
                    if (!Cubes1[i][j + 1].Checked && CurrentColor == Table1.Rows[i].Cells[j + 1].BackColor.ToString())
                    {

                        topside = true;
                        Fill(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref connectorI,ref connectorJ,ref StartSide);
                        
                    }
                }

                if (i < N - 1)
                {
                    if (!Cubes1[i + 1][j].Checked && CurrentColor == Table1.Rows[i + 1].Cells[j].BackColor.ToString())
                    {

                        topside = true;
                        Fill(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                       
                    }
                }

                if (j > 0)
                {
                    if (!Cubes1[i][j - 1].Checked && CurrentColor == Table1.Rows[i].Cells[j - 1].BackColor.ToString())
                    {

                        topside = true;
                        Fill(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                        
                    }
                }

                if (i > 0)
                {
                    if (!Cubes1[i - 1][j].Checked && CurrentColor == Table1.Rows[i - 1].Cells[j].BackColor.ToString())
                    {

                        topside = true;
                        Fill(i - 1, j, topside, Cubes1, Cubes2, CurrentColor,ref connectorI,ref connectorJ, ref StartSide);
                        
                    }
                }

                

            }
            if (!topside)
            {
               
                Table2.Rows[i].Cells[j].Text = " * ";
                Table2.Rows[i].Cells[j].ForeColor = Color.Black;
                Cubes2[i][j].Checked = true;
                if (!Cubes1[i][j].Checked && CurrentColor == Table1.Rows[i].Cells[j].BackColor.ToString() && Table2.Rows[i].Cells[j].BackColor.ToString() == Table1.Rows[i].Cells[j].BackColor.ToString())
                {
                    topside = true;
                    
                    
                    connectorI = i;
                    connectorJ = j;
                    Fill(i, j, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);

                }

                if (j < M - 1)
                {
                    if (!Cubes2[i][j + 1].Checked && CurrentColor == Table2.Rows[i].Cells[j + 1].BackColor.ToString())
                    {

                        topside = false;
                        Fill(i, j + 1, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                        
                    }
                }

                if (i < N - 1)
                {
                    if (!Cubes2[i + 1][j].Checked && CurrentColor == Table2.Rows[i + 1].Cells[j].BackColor.ToString())
                    {

                        topside = false;
                        Fill(i + 1, j, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                        
                    }
                }

                if (j > 0)
                {
                    if (!Cubes2[i][j - 1].Checked && CurrentColor == Table2.Rows[i].Cells[j - 1].BackColor.ToString())
                    {

                        topside = false;
                        Fill(i, j - 1, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                        
                    }
                }

                if (i > 0)
                {
                    if (!Cubes2[i - 1][j].Checked && CurrentColor == Table2.Rows[i - 1].Cells[j].BackColor.ToString())
                    {

                        topside = false;
                        Fill(i - 1, j, topside, Cubes1, Cubes2, CurrentColor, ref connectorI, ref connectorJ, ref StartSide);
                        
                    }
                }

                

            }

        }
        /// <summary>
        /// Finds if the start of the search for a cube patch begins from the top or bottom table
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="Cubes1"></param>
        /// <param name="Cubes2"></param>
        /// <param name="Table1"></param>
        /// <param name="Table2"></param>
        /// <returns></returns>
        public bool ConfirmaSide(int i,int j, List<List<Cube>> Cubes1, List<List<Cube>> Cubes2, Table Table1, Table Table2)
        {
            if (Table1.Rows[i].Cells[j].BackColor == Table2.Rows[i].Cells[j].BackColor)
                return true;

            int CurrentBlobTop = 1;
            int CurrentBlobBottom=1;
            int MaxBlobTop = 1;
            int MaxBlobBottom = 1;
            int MaxITop = 0;
            int MaxIBottom = 0;
            int MaxJTop = 0;
            int MaxJBottom = 0;

            Search(i,j,true,Cubes1,Cubes2, Table1.Rows[i].Cells[j].BackColor.ToString(),ref MaxBlobTop,ref CurrentBlobTop,ref MaxITop,ref MaxJTop);
            Search(i, j, false, Cubes1, Cubes2, Table2.Rows[i].Cells[j].BackColor.ToString(), ref MaxBlobBottom, ref CurrentBlobBottom, ref MaxIBottom, ref MaxJBottom);

            return MaxBlobTop > MaxBlobBottom;
            
        }
    }
}