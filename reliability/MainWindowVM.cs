using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.IO;
using System.Linq;
using System.Text;
=======
using System.Linq;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD
            R = 2;
=======

>>>>>>> 487169efa03527261078c061938f5605ca22562a
            Lambda = 0.5;
            OnPropertyChanged("N");
            OnPropertyChanged("Lambda");
            OnPropertyChanged("M");
            OnPropertyChanged("Q");
<<<<<<< HEAD
            OnPropertyChanged("R");

            FirstTypeSystem = true;
            
=======

            FirstTypeSystem = true;
            pt = new double[1000];
            //XArray = new List<List<int>>();
            //XArray.Add(new List<int> { 0 });
            //XArray.Add(new List<int> { 1 });
            //XArray.Add(new List<int> { 0, 1 });
            //XArray.Add(new List<int> { 1, 0 });
            //XArray.Add(new List<int> { 1, 1 });
            //XArray.Add(new List<int> { 1, 0 , 0 });
            //XArray.Add(new List<int> { 1, 0, 0 });
            //XArray.Add(new List<int> { 0, 0, 1 });
            //XArray.Add(new List<int> { 0, 1, 0, 0 });
            //XArray.Add(new List<int> { 0, 0, 0, 1 });
            //XArray.Add(new List<int> { 1, 2, 3 });
            //XArray.Add(new List<int> { 1, 3, 4 });
            //XArray.Add(new List<int> { 1, 1, 2 });
            //XArray.Add(new List<int> { 2, 2, 1, 1 });
            //XArray.Add(new List<int> { 3, 4, 2, 1 });
            //XArray.Add(new List<int> { 3, 5, 2, 2 });
            //XArray.Add(new List<int> { 1, 2, 6, 3 });
            //XArray.Add(new List<int> { 3, 3, 2 });
            //XArray.Add(new List<int> { 2, 3, 3 });

>>>>>>> 487169efa03527261078c061938f5605ca22562a
            Zk = new List<KeyValuePair<int, double>>();
            Yk = new List<KeyValuePair<int, double>>();
            Wk = new List<KeyValuePair<int, double>>();
            Ak = new List<KeyValuePair<int, double>>();
            Bk = new List<KeyValuePair<int, double>>();
            Ck = new List<KeyValuePair<int, double>>();
            AStarK = new List<KeyValuePair<int, double>>();
            CStarK = new List<KeyValuePair<int, double>>();
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

<<<<<<< HEAD
        public double T { get; set; }//среднее время жизни системы
        private List<double> pt;
        
=======
        private double T;//среднее время жизни системы
        private double[] pt;

>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD
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
=======
                            double result = Zk.FirstOrDefault(zk => zk.Key == k).Value * (a - k + 1);
                            var tempAk = new KeyValuePair<int, double>(k, result);
                            Ak.Add(tempAk);
                            var tempBk = new KeyValuePair<int, double>(k, Wk.FirstOrDefault(wk => wk.Key == k).Value * b);
                            Bk.Add(tempBk.Key == 0 ? new KeyValuePair<int, double>(k, N) : tempBk);
                            Ck.Add(new KeyValuePair<int, double>(k, tempAk.Value + Bk.FirstOrDefault(bk => bk.Key == k).Value));
                        }
                        T = calculateT();
>>>>>>> 487169efa03527261078c061938f5605ca22562a
                    }
                    if (secondTypeSystem)
                    {
                        for (int k = 1; k <= M; k++)
                        {
                            Yk.Add(new KeyValuePair<int, double>(k, calculateYk(k)));
                        }
                        for (int k = 1; k <= M; k++)
                        {
<<<<<<< HEAD
                            AStarK.Add(new KeyValuePair<int, double>(k, Yk.Single(yk => yk.Key == k).Value * (a - k + 1)));
                            CStarK.Add(new KeyValuePair<int, double>(k, a - k + 1));
                        }
                        CStarK.Add(new KeyValuePair<int, double>(M + 1, a - (M + 1) + 1));

                        T = calculateTSecond();
                        //pt = 
                        calculatePTSecond();
                    }
                    OnPropertyChanged("T");
                });
            }
        }
        
=======
                            AStarK.Add(new KeyValuePair<int, double>(k, Yk.FirstOrDefault(yk => yk.Key == k).Value * (a - k + 1)));
                            CStarK.Add(new KeyValuePair<int, double>(k, a - k + 1));
                        }
                        T = calculateTSecond();
                    }

                    MessageBox.Show(string.Format("T = {0}", T));
                });
            }
        }

>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD

=======
            
>>>>>>> 487169efa03527261078c061938f5605ca22562a
            return result * 1 / combination(a, k);
        }

        private double calculateWk(int k)
        {
<<<<<<< HEAD
            for (int q = 1; q <= M; q++)
            {
                if ((k >= ((q - 1) * s + 1)) && (k <= q * s))
                {
                    return q;
=======
            for (int i = 0; i <= Q - 1; i++)
            {
                if (k >= (i - 1) * s + 1 && k <= M)
                {
                    return i;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD

            for (int i = 0; i <= a - k + 1; i++)
=======
            
            for (int i = 0; i < a - k + 1; i++)
>>>>>>> 487169efa03527261078c061938f5605ca22562a
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

<<<<<<< HEAD
            for (int k = 1; k <= M; k++)
            {
                for (int i = 1; i <= k + 1; i++)
                {
                    result += getMultiplyAk(k) / (Ck.Single(ck => ck.Key == i).Value * getMultiplyCC(k, i));
                }
            }

            result += 1 / Ck.Single(ck => ck.Key == 1).Value;

            return result / Lambda;
=======
            for (int k = 1; k < M; k++)
            {
                for (int i = 1; i < k + 1; i++)
                {
                    result += getMultiplyAk(k) / Ck.FirstOrDefault(ck => ck.Key == i).Value * getMultiplyCC(k, i);
                }
            }

            result += 1 / Ck.FirstOrDefault(ck => ck.Key == 1).Value;

            return result * 1 / Lambda;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
        }

        private double getMultiplyAk(int k)
        {
            double result = 1;
<<<<<<< HEAD
            for (int j = 1; j <= k; j++)
            {
                result *= Ak.Single(ak => ak.Key == j).Value;
=======
            for (int j = 1; j < k; j++)
            {
                result *= Ak.FirstOrDefault(ak => ak.Key == j).Value;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
            }
            return result;
        }

        private double getMultiplyCC(int k, int i)
        {
            double result = 1;
<<<<<<< HEAD
            for (int j = 1; j <= k + 1; j++)
            {
                if (j != i)
                {
                    result *= Ck.Single(ak => ak.Key == j).Value - Ck.Single(ak => ak.Key == i).Value;
=======
            for (int j = 1; j < k + 1; j++)
            {
                if (j != i)
                {
                    result *= Ck.FirstOrDefault(ak => ak.Key == j).Value - Ck.FirstOrDefault(ak => ak.Key == i).Value;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
                }
            }
            return result;
        }

        private double calculateTSecond()
        {
            double result = 0;

<<<<<<< HEAD
            for (int k = 1; k <= M; k++)
            {
                for (int i = 1; i <= k + 1; i++)
                {
                    result += getMultiplyAStark(k) / (CStarK.Single(ck => ck.Key == i).Value * getMultiplyCStarC(k, i));
                }
            }

            result += 1 / CStarK.Single(ck => ck.Key == 1).Value;
=======
            for (int k = 1; k < M; k++)
            {
                for (int i = 1; i < k + 1; i++)
                {
                    result += getMultiplyAStark(k) / CStarK.FirstOrDefault(ck => ck.Key == i).Value * getMultiplyCStarC(k, i);
                }
            }

            result += 1 / CStarK.FirstOrDefault(ck => ck.Key == 1).Value;
>>>>>>> 487169efa03527261078c061938f5605ca22562a

            return result * 1 / Lambda;
        }

        private double getMultiplyAStark(int k)
        {
            double result = 1;
<<<<<<< HEAD
            for (int j = 1; j <= k; j++)
            {
                result *= AStarK.Single(ak => ak.Key == j).Value;
=======
            for (int j = 1; j < k; j++)
            {
                result *= AStarK.FirstOrDefault(ak => ak.Key == j).Value;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
            }
            return result;
        }

        private double getMultiplyCStarC(int k, int i)
        {
            double result = 1;
<<<<<<< HEAD
            for (int j = 1; j <= k + 1; j++)
            {
                if (j != i)
                {
                    result *= CStarK.Single(ak => ak.Key == j).Value - CStarK.Single(ak => ak.Key == i).Value;
=======
            for (int j = 1; j < k + 1; j++)
            {
                if (j != i)
                {
                    result *= CStarK.FirstOrDefault(ak => ak.Key == j).Value - CStarK.FirstOrDefault(ak => ak.Key == i).Value;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
                }
            }
            return result;
        }

<<<<<<< HEAD
=======
        #endregion

>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD

=======
            
>>>>>>> 487169efa03527261078c061938f5605ca22562a
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
<<<<<<< HEAD

        private List<double> calculatePT()
        {
            List<double> result = new List<double>();
            result.Add(1);
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
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
                stringBuilder1.Append(t).AppendLine();
                stringBuilder2.Append(tempResult).AppendLine();
            }
            File.WriteAllText("result1.txt", stringBuilder1.ToString());
            File.WriteAllText("result2.txt", stringBuilder2.ToString());

            return result;
        }

        private List<double> calculatePTSecond()
        {
            List<double> result = new List<double>();
            result.Add(1);
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
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
                stringBuilder1.Append(t).AppendLine();
                stringBuilder2.Append(tempResult).AppendLine();
            }
            File.WriteAllText("result1.txt", stringBuilder1.ToString());
            File.WriteAllText("result2.txt", stringBuilder2.ToString());

            return result;
        }

        #endregion

=======
        
>>>>>>> 487169efa03527261078c061938f5605ca22562a
    }

}
