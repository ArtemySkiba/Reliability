using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace reliability
{
    class MainWindowVM : NotifyPropertyChanged
    {

        #region Fields

        private List<List<int>> XArray;//распределение отказавших элементов

        #endregion

        #region Ctor

        public MainWindowVM()
        {
            N = 10;
            M = 2;
            Q = 2;
            R = 2;
            Lambda = 0.5;
            OnPropertyChanged("N");
            OnPropertyChanged("Lambda");
            OnPropertyChanged("M");
            OnPropertyChanged("Q");
            OnPropertyChanged("R");

            FirstTypeSystem = true;

            Zk = new List<KeyValuePair<int, double>>();
            Yk = new List<KeyValuePair<int, double>>();
            Wk = new List<KeyValuePair<int, double>>();
            Ak = new List<KeyValuePair<int, double>>();
            Bk = new List<KeyValuePair<int, double>>();
            Ck = new List<KeyValuePair<int, double>>();
            AStarK = new List<KeyValuePair<int, double>>();
            CStarK = new List<KeyValuePair<int, double>>();
            ptResults = new List<PtResult>();
        }

        #endregion

        #region Input data

        public int N { get; set; }//количество основных элементов

        public int M { get; set; }//количество резервных элементов

        public int Q { get; set; }//количество групп

        public int R { get; set; }//число отказавших элементов, расположенных подряд

        public double Lambda { get; set; }//интенсивность отказов

        private bool firstTypeSystem;
        public bool FirstTypeSystem
        {
            get { return firstTypeSystem; }
            set
            {
                if (firstTypeSystem != value)
                {
                    firstTypeSystem = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FirstTypeSystemVisibility");
                    OnPropertyChanged("SecondTypeSystemVisibility");
                }
            }
        }

        private bool secondTypeSystem;
        public bool SecondTypeSystem
        {
            get { return secondTypeSystem; }
            set
            {
                if (secondTypeSystem != value)
                {
                    secondTypeSystem = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FirstTypeSystemVisibility");
                    OnPropertyChanged("SecondTypeSystemVisibility");
                }
            }
        }

        public Visibility FirstTypeSystemVisibility
        {
            get { return firstTypeSystem ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility SecondTypeSystemVisibility
        {
            get { return secondTypeSystem ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Calculate data

        private int s;//количество резервных элементов в группе
        private int b;//количество основных элементов в группе
        private int a;//общее количество элементов в системе

        private List<KeyValuePair<int, double>> Zk;//отношение числа возможностей попадания системы А в состояние 
        //Sk к числу вероятностей паоадния системы А в это же состояние
        private List<KeyValuePair<int, double>> Yk;
        private List<KeyValuePair<int, double>> Wk;

        private List<KeyValuePair<int, double>> Ak;
        private List<KeyValuePair<int, double>> Bk;
        private List<KeyValuePair<int, double>> Ck;

        private List<KeyValuePair<int, double>> AStarK;
        private List<KeyValuePair<int, double>> CStarK;

        public double T { get; set; }//среднее время жизни системы
        private List<double> pt;
        private List<PtResult> ptResults;

        #endregion

        #region Commands

        private ICommand calculateCommand;
        public ICommand CalculateCommand
        {
            get
            {
                return calculateCommand = calculateCommand ?? new Command(() =>
                {
                    Zk.Clear();
                    Wk.Clear();
                    Ak.Clear();
                    Bk.Clear();
                    Ck.Clear();
                    Yk.Clear();
                    AStarK.Clear();
                    CStarK.Clear();

                    //1.
                    s = M / Q;
                    b = N / Q;
                    a = N + M;

                    XArray = generateXs();
                    //2.
                    if (firstTypeSystem)
                    {
                        for (int k = 1; k <= M; k++)
                        {
                            Zk.Add(new KeyValuePair<int, double>(k, calculateZk(k)));
                            Wk.Add(new KeyValuePair<int, double>(k, calculateWk(k)));
                        }

                        for (int k = 1; k <= M; k++)
                        {
                            double result = Zk.Single(zk => zk.Key == k).Value * (a - k + 1);
                            Ak.Add(new KeyValuePair<int, double>(k, result));
                            Bk.Add(new KeyValuePair<int, double>(k, Wk.Single(wk => wk.Key == k).Value * b));
                        }
                        Bk.Add(new KeyValuePair<int, double>(M + 1, N));

                        for (int k = 1; k <= M; k++)
                        {
                            Ck.Add(new KeyValuePair<int, double>(k, Ak.Single(ak => ak.Key == k).Value +
                                Bk.Single(bk => bk.Key == k).Value));
                        }
                        Ck.Add(new KeyValuePair<int, double>(M + 1, Bk.Single(bk => bk.Key == M + 1).Value));

                        T = calculateT();
                        //pt = 
                        calculatePT();
                    }
                    if (secondTypeSystem)
                    {
                        for (int k = 1; k <= M; k++)
                        {
                            Yk.Add(new KeyValuePair<int, double>(k, calculateYk(k)));
                        }
                        for (int k = 1; k <= M; k++)
                        {
                            AStarK.Add(new KeyValuePair<int, double>(k, Yk.Single(yk => yk.Key == k).Value * (a - k + 1)));
                            CStarK.Add(new KeyValuePair<int, double>(k, a - k + 1));
                        }
                        CStarK.Add(new KeyValuePair<int, double>(M + 1, a - (M + 1) + 1));

                        T = calculateTSecond();
                        //pt = 
                        calculatePTSecond();
                    }
                    OnPropertyChanged("T");

                    ReportViewreControl reportViewreControl = new ReportViewreControl(ptResults);
                    //reportViewreControl.Show();
                });
            }
        }

        #endregion

        #region Methods

        private double calculateZk(int k)
        {
            if (k >= 1 && k <= s)
            {
                return 1;
            }

            double result = 0;

            foreach (var xarray in XArray.Where(x => x.Sum() == k))
            {
                double tempResult = 1;
                for (int i = 1; i < Q; i++)
                {
                    tempResult *= combination(b, xarray[i]);
                }
                result += tempResult;
            }

            return result * 1 / combination(a, k);
        }

        private double calculateWk(int k)
        {
            for (int q = 1; q <= M; q++)
            {
                if ((k >= ((q - 1) * s + 1)) && (k <= q * s))
                {
                    return q;
                }
            }
            throw new Exception("WTF");
        }

        private double calculateYk(int k)
        {
            double result = 0;

            if (k >= 1 && k <= R)
            {
                return 1;
            }

            for (int i = 0; i <= a - k + 1; i++)
            {
                result += Math.Pow(-1, i) * combination(a - k + 1, i) * combination(a - (R + 1) * i, k - (R + 1) * i);
            }
            return result * 1 / combination(a, k);
        }

        private long combination(long n, long k)
        {
            double sum = 0;
            for (long i = 0; i < k; i++)
            {
                sum += Math.Log10(n - i);
                sum -= Math.Log10(i + 1);
            }
            return (long)Math.Pow(10, sum);
        }

        private double calculateT()
        {
            double result = 0;

            for (int k = 1; k <= M; k++)
            {
                for (int i = 1; i <= k + 1; i++)
                {
                    result += getMultiplyAk(k) / (Ck.Single(ck => ck.Key == i).Value * getMultiplyCC(k, i));
                }
            }

            result += 1 / Ck.Single(ck => ck.Key == 1).Value;

            return result / Lambda;
        }

        private double getMultiplyAk(int k)
        {
            double result = 1;
            for (int j = 1; j <= k; j++)
            {
                result *= Ak.Single(ak => ak.Key == j).Value;
            }
            return result;
        }

        private double getMultiplyCC(int k, int i)
        {
            double result = 1;
            for (int j = 1; j <= k + 1; j++)
            {
                if (j != i)
                {
                    result *= Ck.Single(ak => ak.Key == j).Value - Ck.Single(ak => ak.Key == i).Value;
                }
            }
            return result;
        }

        private double calculateTSecond()
        {
            double result = 0;

            for (int k = 1; k <= M; k++)
            {
                for (int i = 1; i <= k + 1; i++)
                {
                    result += getMultiplyAStark(k) / (CStarK.Single(ck => ck.Key == i).Value * getMultiplyCStarC(k, i));
                }
            }

            result += 1 / CStarK.Single(ck => ck.Key == 1).Value;

            return result * 1 / Lambda;
        }

        private double getMultiplyAStark(int k)
        {
            double result = 1;
            for (int j = 1; j <= k; j++)
            {
                result *= AStarK.Single(ak => ak.Key == j).Value;
            }
            return result;
        }

        private double getMultiplyCStarC(int k, int i)
        {
            double result = 1;
            for (int j = 1; j <= k + 1; j++)
            {
                if (j != i)
                {
                    result *= CStarK.Single(ak => ak.Key == j).Value - CStarK.Single(ak => ak.Key == i).Value;
                }
            }
            return result;
        }

        public List<List<int>> generateXs()
        {
            List<string> qs = new List<string>();
            for (int i = 1; i <= Q; i++)
            {
                qs.Add(i.ToString());
            }
            String[] NMass = qs.ToArray();//Q

            List<string> ns = new List<string>();
            for (int i = 0; i <= s; i++)
            {
                ns.Add(i.ToString());
            }
            String[] MMass = ns.ToArray();//0 ..... S

            String[][] Help = new String[NMass.Length][];
            for (int i = 0; i < NMass.Length; i++)
                Help[i] = MMass;

            String[] Variants = null;
            foreach (String[] str in Help)
                Variants = Variants == null ? str :
                    Variants.SelectMany(x => str, (x, y) => x + "  " + y).ToArray();

            List<List<int>> array = new List<List<int>>();
            foreach (var variant in Variants)
            {
                List<int> tempResult = new List<int>();
                foreach (var v in variant.Where(char.IsDigit))
                {
                    tempResult.Add(Convert.ToInt32(v.ToString()));
                }
                array.Add(tempResult);

            }
            return array;
        }

        private void calculatePT()
        {
            List<double> result = new List<double>();
            result.Add(1);
            //StringBuilder stringBuilder1 = new StringBuilder();
            //StringBuilder stringBuilder2 = new StringBuilder();
            for (double t = 0; result.Last() > 0.001; t += 0.001)
            {
                double tempResult = 0;
                for (int k = 1; k <= M; k++)
                {
                    for (int i = 1; i <= k + 1; i++)
                    {
                        tempResult += (getMultiplyAk(k) / getMultiplyCC(k, i)) *
                            Math.Exp(-Ck.Single(ck => ck.Key == i).Value * Lambda * t);
                    }
                }
                tempResult += Math.Exp(-Ck.Single(ck => ck.Key == 1).Value * Lambda * t);
                result.Add(tempResult);
                //stringBuilder1.Append(t).AppendLine();
                //stringBuilder2.Append(tempResult).AppendLine();
                ptResults.Add(new PtResult { T = t, Value = tempResult });
            }
            //File.WriteAllText("result1.txt", stringBuilder1.ToString());
            //File.WriteAllText("result2.txt", stringBuilder2.ToString());
        }

        private void calculatePTSecond()
        {
            List<double> result = new List<double>();
            result.Add(1);
            //StringBuilder stringBuilder1 = new StringBuilder();
            //StringBuilder stringBuilder2 = new StringBuilder();
            for (double t = 0; result.Last() > 0.001; t += 0.001)
            {
                double tempResult = 0;
                for (int k = 1; k <= M; k++)
                {
                    for (int i = 1; i <= k + 1; i++)
                    {
                        tempResult += (getMultiplyAStark(k) / getMultiplyCStarC(k, i)) *
                            Math.Exp(-CStarK.Single(ck => ck.Key == i).Value * Lambda * t);
                    }
                }
                tempResult += Math.Exp(-CStarK.Single(ck => ck.Key == 1).Value * Lambda * t);
                result.Add(tempResult);
                //stringBuilder1.Append(t).AppendLine();
                //stringBuilder2.Append(tempResult).AppendLine();
                ptResults.Add(new PtResult { T = t, Value = tempResult });
            }
            //File.WriteAllText("result1.txt", stringBuilder1.ToString());
            //File.WriteAllText("result2.txt", stringBuilder2.ToString());
        }

        #endregion

    }

}
