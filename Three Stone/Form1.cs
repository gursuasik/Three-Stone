using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Three_Stone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
/*
 * 
 * 0=>You
 * 1=>CPU
 * 2=>Void
 * 3=>AVoid
 */
        int[][,,] Stones = new int[4][,,];//Taşların dizilişi bulunur.
        int[] ACnt = new int[5];//hamle sayısını tutar.
        bool TS = true;//bilgisayarın ilk hamlesinde TS varmı diye bakar
        int Mode = 7;//Oyunun modunu belirler.  0=1=2=3=4=5=Put, 6=Move, 7=Wait,
        int StnX, StnY;//Stone koodinatları tutulur.
        Random R=new Random();
        int Red0, Green0, Blue0, Red1, Green1, Blue1, RedL, GreenL, BlueL;
        private void SelectColor()
        {
            Red0=R.Next(0,256);
            Green0=R.Next(0,256);
            Blue0=R.Next(0,256);
            Red1=R.Next(0,256);
            Green1=R.Next(0,256);
            Blue1=R.Next(0,256);
            RedL=R.Next(0,256);
            GreenL=R.Next(0,256);
            BlueL=R.Next(0,256);
            this.BackColor = Color.FromArgb(R.Next(0, 256), R.Next(0, 256), R.Next(0, 256));
            label1.ForeColor = Color.FromArgb(R.Next(0, 256), Red0, Green0, Blue0);
            label2.ForeColor = Color.FromArgb(R.Next(0, 256),Red1, Green1, Blue1);
            label3.ForeColor=Color.FromArgb(R.Next(0, 256),R.Next(0, 256), R.Next(0, 256), R.Next(0, 256));
        }
        private void GDLine()
        {
            Graphics DLine;
            DLine = this.CreateGraphics();
            Pen PL=new Pen(Color.FromArgb(255,RedL,GreenL,BlueL),6);
            DLine.Clear(this.BackColor);
            DLine.DrawLine(PL,100,100,400,400);
            DLine.DrawLine(PL,400,100,100,400);
            DLine.DrawLine(PL,250,100,250,400);
            DLine.DrawLine(PL,400,250,100,250);
            DLine.DrawLine(PL,100,100,100,400);
            DLine.DrawLine(PL,400,100,400,400);
            DLine.DrawLine(PL,100,100,400,100);
            DLine.DrawLine(PL,100,400,400,400);
            DLine.Dispose();
        }
        private void GDEllipse()
        {
            GDLine();
            Graphics DEllipse;
            DEllipse = this.CreateGraphics();
            for (int i = 0; i < 3; i++)
			{
                for (int j = 0; j < 3; j++)
			    {
                    for (int r = 0; r < 50; r++)
			        {
                        if (Stones[3][0,i,j]==0)
	                    {
                            Pen PE = new Pen(Color.FromArgb(255-r, (Red0 +r+75)%255, (Green0 +r+75)%255, (Blue0+r+75)%255), 1);
                            DEllipse.DrawEllipse(PE,100+150*i-r/2,100+150*j-r/2,r,r);		 
	                    }
                        if (Stones[3][0,i,j]==1)
	                    {
                            Pen PE = new Pen(Color.FromArgb(255-r, (Red1+r) % 255, (Green1+r) %255, (Blue1+r) % 255), 1);
                            DEllipse.DrawEllipse(PE,100+150*i-r/2,100+150*j-r/2,r,r);		 
	                    }
                        /*
                        if (Stones[3][0,i,j]==3)
	                    {
                            Pen PE = new Pen(Color.FromArgb(100-r, (Red0 +r+75)%255, (Green0 +r+75)%255, (Blue0+r+75)%255), 1);
                            DEllipse.DrawEllipse(PE,100+150*i-r/2,100+150*j-r/2,r,r);		 
	                    }
                         */
                    }
			    }
			}
            //DEllipse.Dispose();
        }
        private void Starting()
        {
            for (int Q = 0; Q < 3; Q++)
            {
                for (int A = 0; A < 2000; A++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Stones[Q][A, i, j] = 2;
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
			{
			    for (int j = 0; j < 3; j++)
			    {
			        Stones[3][0,i,j]=2;
			    }
			}
            SelectColor();
       }
        private void CPUAttack()
        {
//MessageBox.Show(Convert.ToString(Mode));
            for (int i = 0; i < 5; i++)
            {
                ACnt[i] = 0;
            }
            AStone(0, 0, 3, 0);//geçici olarak fonksiyonda kullanma kolaylığı sağlar
            ACnt[0] = 1;
            if (Mode == 6)//burayı kontrol et
            {
                AttackMoveStone(1);
            }
            else if (Mode<6)
            {
                AttackPutStone(1);  
            }
            TS = true;
            ACnt[2] =0;//TS hamle sayısını alır
            ACnt[3] =0;//sadece hamle sayısı
            for (int AQ = 0; AQ < ACnt[1]; AQ++)
            {
                if (ThreeStone(1,AQ,1))//CPU ilk hamlede TS varmı
                {
                    AStone(2, ACnt[2], 1, AQ);
                    ACnt[2] += 1;
                    TS = false;
                }
                else if (TS)
                {
                    AStone(2, ACnt[3], 1, AQ);
                    ACnt[3] += 1;
                }
            }
            if (TS)
            {
                ACnt[4] = ACnt[3];
                ACnt[3] = 0;
                for (int AC = 0; AC < ACnt[4]; AC++)//yukarıdaki hamlede TS yoksa sonraki hamleleri kontrol eder
                {//ilk atamada TS kontrolünü yap
                    TS = true;
                    ACnt[1] = 1;//ilk dalgalanmayı atamak için
                    AStone(1, 0, 2, AC);
                    for (int ACC = 0; ACC < 4 && TS; ACC++)//sıradaki dalganın 5 hamle sonrası dalgalanmayı sağlar
                    {
                        if (Mode ==6)//burayı sen yinede bi kontrol et ve yukardakinide
                        {
                            AttackMoveStone(ACC % 2);
                        }
                        else if ((Mode + ACC) < 6)//sanırsam oldu ama yine de kontrol et
                        {
                            AttackPutStone(ACC % 2);
                        }
                        for (int AQ = 0; AQ < ACnt[ACC % 2] && TS; AQ++)//bulunduğu dalgada TS varmı diye bakar
                        {
                            if (ThreeStone(1, AQ,1))//CPU ilk hamlede TS varmı
                            {
                                AStone(2, ACnt[2], 2, AC);
                                ACnt[2] += 1;
                                TS = false;
                            }
                            if (ThreeStone(0, AQ,0))//kullanıcı ilk hamlede TS yoksa
                            {
                                TS = false;
                            }
                        }
                    }
                    if (TS)
                    {
                        AStone(2, ACnt[3], 2, AC);//Eror (1 dolduğunda onu tutmak için 2 ye atıyor..
                        ACnt[3] += 1;
                    }
                }
            }
            if (ACnt[2]>0)
            {
                ACnt[3] = ACnt[2];
            }
            AStone(3, 0, 2, R.Next(0, ACnt[3]));
            GDEllipse();
            if (ThreeStone(2, 0,1))
            {
                GameWin(1);
            }
        }
        private void AttackPutStone(int APQueue)//tamam
        {
            ACnt[APQueue] = 0;
            for (int APQ = 0; APQ < ACnt[(APQueue+1)%2]; APQ++)//0 daki attack sayısı kadar döner
            {
                for (int i = 0; i < 3; i++)
			    {
			        for (int j = 0; j < 3; j++)
			        {
                        if (Stones[(APQueue + 1) % 2][APQ, i, j] == 2)//dizi 0 a bakar
                        {
                            AStone(APQueue, ACnt[APQueue], (APQueue + 1) % 2, APQ);
                            Stones[APQueue][ACnt[APQueue], i, j] = APQueue;//dizi 1 e kaydeder
                            ACnt[APQueue] += 1;
                        }
			        }
			    }            
            }
        }
        private void AttackMoveStone(int AMQueue)//yeni arrayi düzenle
        {
            ACnt[AMQueue] = 0;
            for (int AMQ = 0; AMQ < ACnt[(AMQueue+1)%2]; AMQ++)//0 daki attack sayısı kadar döner
            {
                for (int i = 0; i < 3; i++)
			    {
			        for (int j = 0; j < 3; j++)
			        {
			            if (Stones[(AMQueue + 1) % 2][AMQ,i,j]==AMQueue)//==1
	                    {
                            if (j - 1 > -1)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i, j - 1] == 2 && j - 1 > -1)// |
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i, j - 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i + 1 < 3 && j - 1 > -1)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i + 1, j - 1] == 2 && i + 1 < 3 && j - 1 > -1 && !((i == 0 && j == 1) || (i == 1 && j == 2)))// /
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i + 1, j - 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i + 1 < 3)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i + 1, j] == 2 && i + 1 < 3)// -
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i + 1, j] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i + 1 < 3 && j + 1 < 3)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i + 1, j + 1] == 2 && i + 1 < 3 && j + 1 < 3 && !((i == 1 && j == 0) || (i == 0 && j == 1)))// \
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i + 1, j + 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (j + 1 < 3)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i, j + 1] == 2 && j + 1 < 3)// |
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i, j + 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i - 1 > -1 && j + 1 < 3)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i - 1, j + 1] == 2 && i - 1 > -1 && j + 1 < 3 && !((i == 1 && j == 0) || (i == 2 && j == 1)))// /
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i - 1, j + 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i - 1 < -1)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i - 1, j] == 2 && i - 1 < -1)// -
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i - 1, j] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
                            if (i - 1 > -1 && j - 1 > -1)
                            {
                                if (Stones[(AMQueue + 1) % 2][AMQ, i - 1, j - 1] == 2 && i - 1 > -1 && j - 1 > -1 && !((i == 1 && j == 2) || (i == 2 && j == 1)))// \
                                {
                                    AStone(AMQueue, ACnt[AMQueue], (AMQueue + 1) % 2, AMQ);
                                    Stones[AMQueue][ACnt[AMQueue], i, j] = 2;
                                    Stones[AMQueue][ACnt[AMQueue], i - 1, j - 1] = AMQueue;
                                    ACnt[AMQueue] += 1;
                                }
                            }
			            }
			        }            
                }
	        }
        }
        private void AStone(int Q0, int AQ0,int Q1,int AQ1)
        {
            for (int ik = 0; ik < 3; ik++)
            {
                for (int jl = 0; jl < 3; jl++)
                {
                    Stones[Q0][AQ0, ik, jl] = Stones[Q1][AQ1, ik, jl];//dizi 1 e kaydeder
                }
            }
        }
        private bool ThreeStone(int TSQueue, int TSAC, int TSQ)
        {
            if ((Stones[TSQueue][TSAC,0, 0] == TSQ && Stones[TSQueue][TSAC,1, 1] == TSQ && Stones[TSQueue][TSAC,2, 2] == TSQ) || (Stones[TSQueue][TSAC,2, 0] == TSQ && Stones[TSQueue][TSAC,1, 1] == TSQ && Stones[TSQueue][TSAC,0, 2] == TSQ))//Crash
	        {
                return true;
            }
            else
	        {
                for (int i = 0; i < 3; i++)
                {
                    if (Stones[TSQueue][TSAC,i, 0] == TSQ && Stones[TSQueue][TSAC,i, 1] == TSQ && Stones[TSQueue][TSAC,i, 2] == TSQ)//Horizontel
                    {
                        return true;
                    }
                    if (Stones[TSQueue][TSAC,0, i] == TSQ && Stones[TSQueue][TSAC,1, i] == TSQ && Stones[TSQueue][TSAC,2, i] == TSQ)//Vertical
                    {
                        return true;
                    }
                }
	        }
            return false;
        }
        private void GameWin(int GWQueue)
        {
            Mode = 7;
            if (GWQueue==0)
            {
                label1.Text=Convert.ToString(Convert.ToInt16(label1.Text)+1);
                MessageBox.Show("Congratulations, You Win");
            }
            if (GWQueue==1)
            {
                label2.Text=Convert.ToString(Convert.ToInt16(label2.Text)+1);
                MessageBox.Show("Congratulations, CPU Win");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Stones[0] = new int[2000, 3, 3];
            Stones[1] = new int[2000, 3, 3];
            Stones[2] = new int[2000, 3, 3];
            Stones[3] = new int[1, 3, 3];
            Mode = 7;
            Starting();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            GDLine();
            GDEllipse();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 3 && Mode >5; i++)
            {
                for (int j = 0; j < 3 && Mode >5; j++)
                {
                    if (Stones[3][0,i, j] == 0 && Math.Abs(100 + 150 * i - e.X) < 26 && Math.Abs(100 + 150 * j - e.Y) < 26)
                    {
                        StnX = i;
                        StnY = j;
                    }
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 3 && Mode!=7; i++)
            {
                for (int j = 0; j < 3 && Mode != 7; j++)
                {
                    if (Stones[3][0,i, j] == 2 && Math.Abs(100 + 150 * i - e.X) < 26 && Math.Abs(100 + 150 * j - e.Y) < 26)
                    {
                        Stones[3][0,i, j] = 0;
                        if (Mode==6)
                        {
                            Stones[3][0,StnX, StnY] = 2;
                            GDEllipse();
                            if (ThreeStone(3, 0,0))
                            {
                                GameWin(0);
                            }
                            else
                            {
                                CPUAttack();
                            }
                        }
                        if (Mode<6)
                        {
                            Graphics DEllipse;
                            DEllipse = this.CreateGraphics();
                            for (int r = 0; r < 50; r++)
                            {
                                System.Threading.Thread.Sleep(10);//gecikmeli çalışmasını sağlar
                                Pen PE = new Pen(Color.FromArgb(255 - r, (Red0 + r + 75) % 255, (Green0 + r + 75) % 255, (Blue0 + r + 75) % 255), 1);
                                DEllipse.DrawEllipse(PE, 100 + 150 * i - r / 2, 100 + 150 * j - r / 2, r, r);
                            }
                            GDEllipse();
                            Mode += 1;
                            if (ThreeStone(3,0,0))
                            {
                                GameWin(0);
                            }
                            else
	                        {                                
                                CPUAttack();                               
                                Mode += 1;
	                        }
                        }
                    }
                }
            }
        }
        /*private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int p = 0;
            for (int i = 0; i < 3; i++)
			{
                for (int j = 0; j < 3; j++)
			    {
                    if (p==0 && Stones[3][0,i,j]==2 && Math.Abs(100+150*i-e.X)<26 && Math.Abs(100+150*j-e.Y)<26)
                    {
                        //p = 1;
                        Stones[3][0,i,j]=3;
                        GDLine();
                        GDEllipse();
                        //Stones[3][0,i,j]=2;
                        
                    }
                    else
                    {
                        p = 0;
                        GDLine();
                        GDEllipse();
                    }
			    }
			}
        }*/
        private void label3_Click(object sender, EventArgs e)
        {
            Starting();
            GDEllipse();
            Mode= R.Next(0, 2);
            if (Mode ==1)
            {
                CPUAttack();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}