using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Text.RegularExpressions;

public struct Point
{
    public int num;
    public double x, y;

    public Point(int nn, double xx, double yy)
    {
        num = nn;
        x = xx;
        y = yy;
    }
};
struct twopoint
{
   public double x1, x2;
    public twopoint( double xx1, double xx2)
    {
        x1 = xx1;
        x2 = xx2;
    }
};
struct Pointforsys
{
    public int num;
    public double x, y1, y2;
    public Pointforsys(int nn, double xx, double yy1, double yy2)
    {
        num = nn;
        x = xx;
        y1 = yy1;
        y2 = yy2;
    }
};
namespace CMLabKAB1
{
    public partial class Form1 : Form
    {
        int maxnum;
        double u10,u20, h00, E, xn,a,b,c;
        double eboard = 0.05;

        Form2 form = new Form2();

        public Form1()
        {
            InitializeComponent();

           
            h00 = double.Parse(textBox3.Text);
            u10 = double.Parse(textBox9.Text);
            u20 = double.Parse(textBox10.Text);
            maxnum = int.Parse(textBox1.Text);
            E = double.Parse(textBox2.Text);
            xn = double.Parse(textBox4.Text);
            a = double.Parse(textBox11.Text);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                if (checkBox1.Checked == false)
                    ChislwithhlfStep();
                else Conststep();
            }
            if (comboBox1.SelectedIndex == 0)
            {
                if (checkBox1.Checked == false)
                    ChislwithhlfStep();
                else Conststep();
            }
            if (comboBox1.SelectedIndex == 2)
            {

                RezOsn2();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;


            if (comboBox1.SelectedIndex == 1)
            {
                pane.CurveList.Clear();
                textBox10.Visible = false;
                label19.Visible = false;
                pane.Title = "Решение основной задачи";
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                label13.Text = "du1/dx = (x^3+1)/(x^5+1)*u1^2+u1-u1^3*sin10x";
                label12.Text = "Для этой задачи не получится найти точное решение";

            }
            if (comboBox1.SelectedIndex == 0)
            {
                pane.CurveList.Clear();
                pane.Title = "Решение тестовой задачи";
                textBox10.Visible = false;
                label19.Visible = false;
                dataGridView1.Columns[12].Visible = true;
                dataGridView1.Columns[13].Visible = true;


                label13.Text = "du1/dx=2*u1";
                label12.Text = "Решением тестовой задачи будет уравнение вида\nu1 = u0 * e ^ (2 * x) для u1(0)= u0";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                pane.CurveList.Clear();
                textBox10.Visible = true;
                label19.Visible = true;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                label13.Text = "d2u1/dx2 = -au1|u1'| + bu1' + cu1";
                label12.Text = "Представим уравнение в виде системы \n u2=u1'\n u2'=-au2|u2| + bu2 + cu1";
            }

        }
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            form.Show();
        }
        private void DrawExp(Point[] arr, int n)
        {
            GraphPane pane;
            pane = zedGraphControl1.GraphPane;

            PointPairList listexp = new PointPairList();
            for (int i = 0; i < n; i++)
            {
                // добавим в список точку
                
                    listexp.Add(arr[i].x, u10*Math.Exp(2 * arr[i].x));

                    dataGridView1.Rows[i].Cells[12].Value = Math.Round(u10*Math.Exp(2 * arr[i].x), 12);
                    dataGridView1.Rows[i].Cells[13].Value = Math.Round(Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells[12].Value) - arr[i].y),10);

            }
            LineItem myCurve1 = pane.AddCurve("", listexp, Color.Red, SymbolType.VDash);
            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void DrawMain(Point[] arr1, int n)
          {

            GraphPane pane;
            pane = zedGraphControl1.GraphPane;
            pane.XAxis.Title = "X ось";
            pane.YAxis.Title = "U1 ось";
            //pane.CurveList.Clear();
            PointPairList list = new PointPairList();
            for (int i = 0; i < n; i++)
            {
                // добавим в список точку
               
                list.Add(arr1[i].x, arr1[i].y);

            }
            LineItem myCurve = pane.AddCurve("", list, Color.Indigo, SymbolType.None);
            
              zedGraphControl1.AxisChange();
              zedGraphControl1.Invalidate();
          }

        private void DrawMainSys(Pointforsys[] arr1, int n)
        {
           Random rnd = new Random();
            //pane.CurveList.Clear();
            GraphPane pane1 = zedGraphControl1.GraphPane;
            GraphPane pane2 = zedGraphControl2.GraphPane;
            GraphPane pane3 = zedGraphControl3.GraphPane;

            pane1.Title = "Зависимость x|u1"; pane2.Title = "Зависимость x|u2";
            pane3.Title = "Зависимость u1|u2";
            pane1.XAxis.Title = "X ось";
            pane2.XAxis.Title = "X ось";
            pane1.YAxis.Title = "U1 ось";
            pane2.YAxis.Title = "U2 ось";
            pane3.YAxis.Title = "U2 ось";
            pane3.XAxis.Title = "U1 ось";
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();

            for (int i = 0; i < n; i++)
            {
                // добавим в список точку

                list1.Add(arr1[i].x, arr1[i].y1);
                list2.Add(arr1[i].x, arr1[i].y2);
                list3.Add(arr1[i].y1, arr1[i].y2);


            }
            LineItem myCurve1 = pane1.AddCurve("", list1, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), SymbolType.None);
            LineItem myCurve2 = pane2.AddCurve("", list2, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), SymbolType.None);
            LineItem myCurve3 = pane3.AddCurve("", list3, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
        }




        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox2.Text == "") return;
            var actual = textBox2.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox2.Text, newText) != 0)
            {
                var sstart = textBox2.SelectionStart;
                textBox2.Text = newText;
                textBox2.SelectionStart = sstart - 1;
            }
            E = double.Parse(textBox2.Text);
        }

        private double testov(double x, double u)
        {
            return 2 * u;
        }
        private double osnov1(double x, double u)
        {
            return ((Math.Pow(x,3)+1)/(Math.Pow(x,5)+1))*Math.Pow(u,2)+u-Math.Pow(u,3)*Math.Sin(10*x);
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            var actual = textBox1.Text;
            var disallowed = @"[^0-9]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox1.Text, newText) != 0)
            {
                var sstart = textBox1.SelectionStart;
                textBox1.Text = newText;
                textBox1.SelectionStart = sstart - 1;
            }
            maxnum = int.Parse(textBox1.Text);
        }

        Point metodRK(double x0, double u0, double h, int num)
        {
            double x, v;
            x = x0;
            v = u0;

            double k1=0, k2=0, k3=0, k4=0;
            if (comboBox1.SelectedIndex == 1)
            {
                k1 = osnov1(x, v);
                k2 = osnov1(x + h / 2.0, v + (h / 2.0) * k1);
                k3 = osnov1(x + h / 2.0, v + (h / 2.0) * k2);
                k4 = osnov1(x + h, v + (h) * k3);
            }
            if (comboBox1.SelectedIndex == 0)
            {
                k1 = testov(x, v);
                k2 = testov(x + h / 2.0, v + (h / 2.0) * k1);
                k3 = testov(x + h / 2.0, v + (h / 2.0) * k2);
                k4 = testov(x + h, v + (h) * k3);
            }
            v = v + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);

            x += h;

            Point st;
            st.num = num;
            st.x = x;
            st.y = v;

            return st;
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if ((textBox9.Text == "") || (textBox9.Text == "-")) return;
            var actual = textBox9.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox9.Text, newText) != 0)
            {
                var sstart = textBox9.SelectionStart;
                textBox9.Text = newText;
                textBox9.SelectionStart = sstart - 1;
            }
            u10 = double.Parse(textBox9.Text);
        }

        

        private void textBox3_TextChanged_1(object sender, EventArgs e)
       {
            if (textBox3.Text == "") return;
            var actual = textBox3.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox3.Text, newText) != 0)
            {
                var sstart = textBox3.SelectionStart;
                textBox3.Text = newText;
                textBox3.SelectionStart = sstart - 1;
            }
            h00 = double.Parse(textBox3.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "") return;
            var actual = textBox4.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox4.Text, newText) != 0)
            {
                var sstart = textBox4.SelectionStart;
                textBox4.Text = newText;
                textBox4.SelectionStart = sstart - 1;
            }
            xn = double.Parse(textBox4.Text);
        }

        void Globerror(Point[] mas,int k)
        {
            double maxgb = 0;
            int num = 0;
            for(int s=0; s<k; s++)
            {
                if (maxgb < Convert.ToDouble(dataGridView1.Rows[s].Cells[13].Value))
                {
                    maxgb = Convert.ToDouble(dataGridView1.Rows[s].Cells[13].Value);
                    num = s;
                }
            }

            form.label18.Text = Convert.ToString(maxgb);
            form.label19.Text= Convert.ToString(mas[num].x);
        }
        void Conststep()
        {
            Point[] mas = new Point[maxnum];
            double[] e = new double[maxnum];
            double en = 0.0;
            e[0] = en;
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = maxnum;
            double x0 = 0.0, u0 = u10, h0 = h00;

            Point t;
            t.num = 0;
            t.x = x0;
            t.y = u0;
            mas[0] = t;

            int i = 1;

            while ((i < maxnum) && (mas[i-1].x<xn))
            {
                Point t1, t12, t2;
                //dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = i - 1;

                //(x(n+1),v(n+1)
                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;

                t1 = metodRK(x0, u0, h0, i);
                dataGridView1.Rows[i].Cells[2].Value = t1.y;

                //(x(n+1/2),y(n+1/2))

                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;
                t12 = metodRK(x0, u0, h0 / 2.0, i);


                //(x(n),Y(n))

                x0 = t12.x;
                u0 = t12.y;

                t2 = metodRK(x0, u0, h0 / 2.0, i);
                dataGridView1.Rows[i].Cells[4].Value = t2.y;




                int p = 4; // порядок метода 
                double S = Math.Abs((t2.y - t1.y) / (Math.Pow(2, p) - 1));
                en = Math.Pow(2, p) * S;
                e[i] = en;
                dataGridView1.Rows[i].Cells[8].Value = e[i];
                dataGridView1.Rows[i].Cells[9].Value = h00;

                mas[i] = t2;
                i++;
            }
            dataGridView1.RowCount = i;
            form.label11.Text = Convert.ToString(i);
            if (comboBox1.SelectedIndex == 1)
            {
                form.label18.Text = "-";
                form.label19.Text = "-";
            }

            for (int d = 0; (d < i) && (mas[d].x < xn); d++)
            {
                dataGridView1.Rows[d].Cells[1].Value = mas[d].x;
                dataGridView1.Rows[d].Cells[6].Value = Convert.ToDouble(dataGridView1.Rows[d].Cells[2].Value) - Convert.ToDouble(dataGridView1.Rows[d].Cells[4].Value);

            }
            DrawMain(mas, i);
            if (comboBox1.SelectedIndex == 0)
            {

                DrawExp(mas, i);
                Globerror(mas, i);
            }

        }
        void ChislwithhlfStep()
        {
            Point[] mas = new Point[maxnum];
            double[] e= new double[maxnum];
            double[] h = new double[maxnum];
            double en = 0;
            e[0] = en;
            dataGridView1.Rows.Clear();

            dataGridView1.RowCount = maxnum;
            double x0 = 0.0, u0 = u10, h0 = h00;
            int indicator = 0;

            h[0] = h0;
            Point t;
            t.num = 0;
            t.x = x0;
            t.y = u0;
            mas[0] = t;

            int i = 1;
            
            while (i < maxnum)
            {
                Point t1, t12, t2;
                //dataGridView1.Rows.Add();
                dataGridView1.Rows[i-1].Cells[0].Value = i-1;

                //(x(n+1),v(n+1)
                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;

                t1 = metodRK(x0, u0, h0, i);
                dataGridView1.Rows[i].Cells[2].Value = t1.y;

                //(x(n+1/2),y(n+1/2))

                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;
                t12 = metodRK(x0, u0, h0 / 2.0, i);


                //(x(n),Y(n))

                x0 = t12.x;
                u0 = t12.y;

                t2 = metodRK( x0, u0, h0 / 2.0, i);
                dataGridView1.Rows[i].Cells[4].Value = t2.y;




                int p = 4; // порядок метода 
                double S = Math.Abs((t2.y - t1.y) / (Math.Pow(2, p) - 1));
                e[i] = S*Math.Pow(2,p);
                dataGridView1.Rows[i].Cells[8].Value = e[i];

                if (S < E / (Math.Pow(2, p + 1)))
                {
                    h0 = 2.0 * h0;
                    mas[i] = t2;
                    h[i] = h0;
                    if (indicator!=11)
                    {
                        dataGridView1.Rows[i].Cells[11].Value = String.Empty;
                        dataGridView1.Rows[i].Cells[10].Value = 1;
                    }

                    if (mas[i].x > xn)
                        break;
                    i++;
                    continue;
                }
                if (S > E)
                {
                    h0 = h0 / 2.0;
                   h[i] = h0;
                    indicator = 11;


                }
                if ((S > E / (Math.Pow(2, p + 1))) && (S < E))
                {
                    mas[i] = t2;
                    h[i] = h0;
                   if (indicator == 11)
                    {
                        dataGridView1.Rows[i].Cells[11].Value = 1;
                        dataGridView1.Rows[i].Cells[10].Value = String.Empty;

                    }
                    indicator = 0;
                    if (mas[i].x > xn)
                        break;
                    i++;
                    continue;
                    
                }
                
            }
            dataGridView1.RowCount = i;
            form.label11.Text = Convert.ToString(i);
            if (comboBox1.SelectedIndex == 1)
            {
                form.label18.Text = "-";
                form.label19.Text = "-";
            }
            Form2Filling(h, e, i, mas);

            for (int d = 0; (d < i) && (mas[d].x < xn); d++)
            {
                dataGridView1.Rows[d].Cells[1].Value = mas[d].x;
                dataGridView1.Rows[d].Cells[9].Value = h[d];
               
                dataGridView1.Rows[d].Cells[6].Value = Convert.ToDouble(dataGridView1.Rows[d].Cells[2].Value) - Convert.ToDouble(dataGridView1.Rows[d].Cells[4].Value);

            }
            DrawMain(mas,i);
            if (comboBox1.SelectedIndex == 0)
            {

                DrawExp(mas, i);
                Globerror(mas,i);
            }
        }


        //u' = z
        // z'' = -az|z| + bz + cu 

        double osnov2y1(double x, double u1, double u2, double a, double b, double c)//f1'
        {
            double mod = u2;
            if (u1 < 0) mod = (-1) * u2;
            double y1 = u2;
            double y2 = (-1) * a * u2 * mod + b * u2 + c * u1;
            return y1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GraphPane pane1 = zedGraphControl1.GraphPane;
            GraphPane pane2 = zedGraphControl2.GraphPane;
            GraphPane pane3 = zedGraphControl3.GraphPane;
            pane1.CurveList.Clear();
            pane2.CurveList.Clear();
            pane3.CurveList.Clear();

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if ((textBox10.Text == "") || (textBox10.Text == "-")) return;
            var actual = textBox10.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox10.Text, newText) != 0)
            {
                var sstart = textBox10.SelectionStart;
                textBox10.Text = newText;
                textBox10.SelectionStart = sstart - 1;
            }
            u20 = double.Parse(textBox10.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GraphPane pane1 = zedGraphControl1.GraphPane;
            GraphPane pane2 = zedGraphControl2.GraphPane;
            GraphPane pane3 = zedGraphControl3.GraphPane;

            double xmin = Convert.ToDouble(textBox5.Text);
            double xmax = Convert.ToDouble(textBox6.Text);
            double ymin = Convert.ToDouble(textBox7.Text);
            double ymax = Convert.ToDouble(textBox8.Text);

            if(tabControl1.SelectedIndex==0)
            {
                pane1.XAxis.Min = xmin;
                pane1.XAxis.Max = xmax;
                pane1.YAxis.Min = ymin;
                pane1.YAxis.Max = ymax;
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            if (tabControl1.SelectedIndex == 1)
            {
                pane2.XAxis.Min = xmin;
                pane2.XAxis.Max = xmax;
                pane2.YAxis.Min = ymin;
                pane2.YAxis.Max = ymax;
                zedGraphControl2.AxisChange();
                zedGraphControl2.Invalidate();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                pane3.XAxis.Min = xmin;
                pane3.XAxis.Max = xmax;
                pane3.YAxis.Min = ymin;
                pane3.YAxis.Max = ymax;
                zedGraphControl3.AxisChange();
                zedGraphControl3.Invalidate();
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if ((textBox12.Text == "") || (textBox12.Text == "-")) return;
            var actual = textBox12.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox12.Text, newText) != 0)
            {
                var sstart = textBox12.SelectionStart;
                textBox12.Text = newText;
                textBox12.SelectionStart = sstart - 1;
            }
           b = double.Parse(textBox12.Text);
        }

        double osnov2y2(double x, double u1, double u2, double a, double b, double c)//f2'
        {
            double mod = u2;
            if (u1 < 0) mod = (-1) * u2;
            double y1 = u2;
            double y2 = (-1) * a * u2 * mod + b * u2 + c * u1;
            return y2;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if ((textBox11.Text == "") || (textBox11.Text == "-")) return;
            var actual = textBox11.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox11.Text, newText) != 0)
            {
                var sstart = textBox11.SelectionStart;
                textBox11.Text = newText;
                textBox11.SelectionStart = sstart - 1;
            }
            a = double.Parse(textBox11.Text);
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if ((textBox13.Text == "") || (textBox13.Text == "-")) return;
            var actual = textBox13.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox13.Text, newText) != 0)
            {
                var sstart = textBox11.SelectionStart;
                textBox13.Text = newText;
                textBox13.SelectionStart = sstart - 1;
            }
            c = double.Parse(textBox13.Text);
        }

        Pointforsys metodRKS(int num, double h, double x, double u1, double u2)
        {
            double v1 = u1;
            double v2 = u2;

            double k1 = 0, k2 = 0;
            //
            k1 = osnov2y1(x + (0.5 - 0.2886751) * h, u1 + (h * 0.25) * k1 + (0.25 - 0.2886751) * h * k2, u2, a, b, c);
            k2 = osnov2y1(x + (0.5 + 0.2886751) * h, u1 + (0.25 + 0.2886751) * h * k1 + k2 * (h * 0.25), u2, a, b, c);
            v1 = v1 + (h / 2) * (k1 + k2);

            k1 = 0; k2 = 0;
            k1 = osnov2y2(x + (0.5 - 0.2886751) * h, u1, u2 + (h * 0.25) * k1 + (0.25 - 0.2886751) * h * k2, a, b, c);
            k2 = osnov2y2(x + (0.5 + 0.2886751) * h, u1, u2 + (0.25 + 0.2886751) * h * k1 + k2 * (h * 0.25), a, b, c);
            v2 = v2 + (h / 2) * (k1 + k2);

            x += h;

            Pointforsys st;
            st.num = num;
            st.x = x;
            st.y1 = v1;
            st.y2 = v2;

            return st;
        }
        void Form2Filling(double[] mas1, double[] mas2, int k, Pointforsys[] pnt)
        {
            double maxh = 0, minh = 110, emax = 0;
            int mah=0, mih=0;
            for (int i = 1; i < k; i++)
            {
                if (maxh < mas1[i])
                {
                    maxh = mas1[i];
                    mah = i;
                }
                if (minh > mas1[i])
                {
                    minh = mas1[i];
                    mih = i;
                }
                if (emax < mas2[i])
                {
                    emax = mas2[i];
                    
                }

            }
            form.label14.Text = Convert.ToString(maxh);
            form.label16.Text = Convert.ToString(minh);
            form.label13.Text = Convert.ToString(Math.Abs(emax));
            form.label15.Text = Convert.ToString(pnt[mah].x);
            form.label17.Text = Convert.ToString(pnt[mih].x);
        }
        void Form2Filling(double[] mas1, double[] mas2, int k, Point[] pnt)
        {
            double maxh=0, minh=110, emax=0;
            int mah=0, mih=0;
            for (int i = 1; i <k; i++)
            {
                if (maxh < mas1[i])
                {
                    maxh = mas1[i];
                    mah = i;
                }
                if (minh > mas1[i])
                {
                    minh = mas1[i];
                    mih = i;
                }
                if (emax < mas2[i])
                {
                    emax = mas2[i];
                }

            }
            form.label14.Text = Convert.ToString(maxh);
            form.label16.Text = Convert.ToString(minh);
            form.label13.Text = Convert.ToString(Math.Abs(emax));
            form.label15.Text = Convert.ToString(pnt[mah].x);
            form.label17.Text = Convert.ToString(pnt[mih].x);
        }

        int Controlofxn(Point[] mas, double[] h, int k)
        {
            double hnew = h[k-1], x0,u0;
            int iter = k;
            while (iter < maxnum && (mas[iter].x <= xn - eboard || mas[iter].x >= xn + eboard))
            {
                hnew = hnew * 0.5;
                mas[iter].x = mas[iter].x + hnew;

                if (mas[k].x < xn + eboard)
                {
                    //Добавление точек/точки в список 
                    Point t1, t2, t12;
                    x0 = mas[iter - 1].x;
                    u0 = mas[iter - 1].y;

                    t1 = metodRK(x0, u0, hnew, iter);
                    dataGridView1.Rows[iter].Cells[2].Value = t1.y;

                    //(x(n+1/2),y(n+1/2))

                    x0 = mas[iter - 1].x;
                    u0 = mas[iter - 1].y;
                    t12 = metodRK(x0, u0, hnew / 2.0, iter);


                    //(x(n),Y(n))

                    x0 = t12.x;
                    u0 = t12.y;

                    t2 = metodRK(x0, u0, hnew / 2.0, iter);
                    dataGridView1.Rows[iter].Cells[4].Value = t2.y;
                    //... 
                    double S = Math.Abs((t2.y - t1.y) / (Math.Pow(2, 4) - 1));
                    double olp = S*Math.Pow(2,4);
                    dataGridView1.Rows[iter].Cells[8].Value = olp;
                    //Запись данных в таблицу 
                    mas[iter] = t2;
                    h[iter] = hnew;
                    hnew *= 2.0;
                    iter++;

                }
            }
            return iter;
        }

        int Controlofxn(Pointforsys[] mas, double[] h, int k)
        {
            double hnew = h[k - 1], prev_x =mas[k-2].x, x=mas[k-1].x, u01,u02;
            int iter = k;

            while (iter < maxnum && (x <= xn - eboard || x >= xn + eboard))
            {
                hnew = hnew * 0.5;
                x = mas[iter].x + hnew;

                if (x < xn + eboard)
                {
                    //Добавление точек/точки в список 
                    Pointforsys t1, t12, t2;
                    dataGridView1.Rows[iter - 1].Cells[0].Value = iter - 1;

                    //(x(n+1),v(n+1))
                    u01 = mas[iter - 1].y1;
                    u02 = mas[iter - 1].y2;

                    t1 = metodRKS(iter, hnew, prev_x, u01, u02);

                    //(x(n+1/2),y(n+1/2))

                    t12 = metodRKS(iter, hnew * (0.5), prev_x, u01, u02);



                    //(x(n),Y(n))	

                    t2 = metodRKS(iter, hnew * (0.5), t12.x, t12.y1, t12.y2);
                    dataGridView1.Rows[iter].Cells[4].Value = t12.y1;
                    dataGridView1.Rows[iter].Cells[5].Value = t12.y2;

                    double en1 = t2.y1 - t1.y1;
                    double en2 = t2.y2 - t1.y2;
                    

                    double S = Math.Abs(Math.Max(en1, en2));

                    int p = 4;
                    double e = S * Math.Pow(2, p);
                    dataGridView1.Rows[iter].Cells[8].Value = e;
                    //Запись данных в таблицу 
                    mas[iter] = t2;
                    h[iter] = hnew;
                    prev_x = x;
                    hnew *= 2.0;
                    iter++;

                }

            }
            return iter;
        }

        void RezOsn2()
        {
            Pointforsys[] mas = new Pointforsys[maxnum];
            Pointforsys[] obh = new Pointforsys[maxnum];
            double[] e;
            e = new double[maxnum];
            e[0] = 0;
            double[] h;
            h = new double[maxnum];

            double  x0=0.0, h0=h00, u01 = u10, u02 = u20;
            //xn - граница отрезка интегрирования
            int n = maxnum,indicator=0;
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = maxnum;


            Pointforsys t;
            t.num = 0;
            t.x = x0;
            t.y1 = u01;
            t.y2 = u02;
            mas[0] = t;
            obh[0] = t;
            h[0] = h00;

            int c1, c2;
            c1 = c2 = 0;
            twopoint e12;
            e12.x1 = 0;
            e12.x2 = 0;

            int i = 1;
            dataGridView1.Rows.Add();

            while (i < n)
            {
                Pointforsys t1, t12, t2;
                dataGridView1.Rows[i-1].Cells[0].Value = i-1;

                //(x(n+1),v(n+1))
                x0 = mas[i-1].x;
                u01 = mas[i-1].y1;
                u02 = mas[i - 1].y2;

                t1 = metodRKS(i, h0, x0, u01, u02);
                obh[i] = t1;

                //(x(n+1/2),y(n+1/2))

                t12 = metodRKS(i, h0 * (0.5), x0, u01, u02);
                


                //(x(n),Y(n))	

                t2 = metodRKS(i, h0 * (0.5), t12.x, t12.y1, t12.y2);
                dataGridView1.Rows[i].Cells[4].Value = t12.y1;
                dataGridView1.Rows[i].Cells[5].Value = t12.y2;

                double en1 = t2.y1 - t1.y1;
                double en2 = t2.y2 - t1.y2;
                e12.x1 = en1;
                e12.x2 = en2;

                double S = Math.Abs(Math.Max(en1, en2));

                int p = 4;
                e[i] = S*Math.Pow(2,p);
                dataGridView1.Rows[i].Cells[8].Value = e[i];

                if (S < E / (Math.Pow(2, p + 1)))
                {
                    h0 = 2.0 * h0;
                    h[i] = h0;
                    mas[i] = t2;
                    
                        dataGridView1.Rows[i].Cells[11].Value = String.Empty;
                        dataGridView1.Rows[i].Cells[10].Value = 1;
                    
                    indicator = 0;
                    if (mas[i].x > xn)
                        break;
                    i++;
                    
                }
                if (S > E)
                {
                    h0 = h0 * (0.5);
                    h[i] = h0;
                    indicator = 11;

                }
                if ((S > E / (Math.Pow(2, p + 1))) && (S < E))
                {
                    mas[i] = t2;
                    h[i] = h0;
                    if (indicator == 11)
                    {
                        dataGridView1.Rows[i].Cells[10].Value = String.Empty;
                        dataGridView1.Rows[i].Cells[11].Value = 1;
                    }
                    indicator=0;
                    if (mas[i].x > xn)
                        break;
                    i++;

                }

            }
            dataGridView1.RowCount = i;
            form.label11.Text = Convert.ToString(i);
            
            Form2Filling(h, e, i,mas);
            for (int d = 0; (d < n) && (mas[d].x < xn); d++)
            {
                dataGridView1.Rows[d].Cells[1].Value = mas[d].x;
                dataGridView1.Rows[d].Cells[2].Value = obh[d].y1;
                dataGridView1.Rows[d].Cells[3].Value = obh[d].y2;

                dataGridView1.Rows[d].Cells[9].Value = h[d];
                dataGridView1.Rows[d].Cells[6].Value = Convert.ToDouble(dataGridView1.Rows[d].Cells[2].Value) - Convert.ToDouble(dataGridView1.Rows[d].Cells[4].Value);
                dataGridView1.Rows[d].Cells[7].Value = Convert.ToDouble(dataGridView1.Rows[d].Cells[3].Value) - Convert.ToDouble(dataGridView1.Rows[d].Cells[5].Value);

                /* cout << i << "     " << mas[i].x << "     " << "(" << obh[i].y1 << " ; " << obh[i].y2 << ")" << "           " << "(" << mas[i].y1 << " ; " << mas[i].y2 << ")" << "         " << e[i] << "      " << h[i] << endl;
                cout << endl;
                cout << endl; */
            }
            DrawMainSys(mas,i);
            
        }

    }
}






