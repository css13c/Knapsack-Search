using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AI_Phase5
{
    public struct item
    {
        public string name;
        public int cost;
        public int value;
        public double ratio;
    };

    class Sack
    {
        private List<item> stuff;
        private int sumCost;
        private int sumVal;
        private int capacity;
        private int itemCount;

        public Sack(List<item> s, int c, int count)
        {
            stuff = new List<item>(s);
            capacity = c;
            sumCost = 0;
            sumVal = 0;
            foreach (var obj in stuff)
            {
                sumCost += obj.cost;
                sumVal += obj.value;
            }
            itemCount = count;
        }
        public Sack(int c)
        {
            stuff = null;
            capacity = 0;
            sumCost = 0;
            sumVal = 0;
            itemCount = 0;
        }
        public Sack(Sack temp)
        {
            stuff = new List<item>(temp.getSack());
            capacity = temp.getCap();
            sumCost = temp.getCost();
            sumVal = temp.getVal();
            itemCount = temp.getCount();
        }
        public void display(StreamWriter some)
        {
            stuff.Sort((x, y) => x.name.CompareTo(y.name));
            some.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", "Item Name", "Item Cost", "Item Value", "Item Ratio");
            some.WriteLine("------------------------------------------------------------------------------------------------------------------");
            foreach (var obj in stuff)
            {
                some.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", obj.name, obj.cost, obj.value, obj.ratio);
            }
            some.WriteLine("------------------------------------------------------------------------------------------------------------------");
            some.WriteLine("Sum of Costs: {0}", sumCost);
            some.WriteLine("Sum of Values: {0}", sumVal);
        }
        public void displayCon()
        {
            stuff.Sort((x, y) => x.name.CompareTo(y.name));
            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", "Item Name", "Item Cost", "Item Value", "Item Ratio");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            foreach (var obj in stuff)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", obj.name, obj.cost, obj.value, obj.ratio);
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Sum of Costs: {0}", sumCost);
            Console.WriteLine("Sum of Values: {0}", sumVal);
        }
        public List<item> getSack()
        {
            return stuff;
        }
        public int getVal()
        {
            return sumVal;
        }
        public int getCost()
        {
            return sumCost;
        }
        public int getCap()
        {
            return capacity;
        }
        public int getCount()
        {
            return itemCount;
        }
    }

    class Program
    {
        //greedy solutions
        static public int HVal(List<item> collection, int capacity)
        {
            //sort by highest value
            collection.Sort((x, y) => y.value.CompareTo(x.value));

            List<item> temp = new List<item>(collection);
            List<item> sack = new List<item>();
            int sackWeight = 0;
            int sum = 0;
            while (temp.Count > 0)
            {
                sackWeight += temp[0].cost;
                if (sackWeight > capacity)
                    break;
                sack.Add(temp[0]);
                sum += temp[0].value;
                temp.RemoveAt(0);
            }
            return sum;
        }
        static public int LCost(List<item> collection, int capacity)
        {
            //sort by lowest cost
            collection.Sort((x, y) => x.cost.CompareTo(y.cost));

            List<item> temp = new List<item>(collection);
            List<item> sack = new List<item>();
            int sackWeight = 0;
            int sum = 0;
            while (temp.Count > 0)
            {
                sackWeight += temp[0].cost;
                if (sackWeight > capacity)
                    break;
                sack.Add(temp[0]);
                sum += temp[0].value;
                temp.RemoveAt(0);
            }
            return sum;
        }
        static public int HRat(List<item> collection, int capacity)
        {
            //sort by ratio
            collection.Sort((x, y) => y.ratio.CompareTo(x.ratio));

            List<item> temp = new List<item>(collection);
            List<item> sack = new List<item>();
            int sackWeight = 0;
            int sum = 0;
            while (temp.Count > 0)
            {
                sackWeight += temp[0].cost;
                if (sackWeight > capacity)
                    break;
                sack.Add(temp[0]);
                sum += temp[0].value;
                temp.RemoveAt(0);
            }
            return sum;
        }
        static public double Partial(List<item> collection, int capacity)
        {
            //sort by ratio
            collection.Sort((x, y) => y.ratio.CompareTo(x.ratio));

            List<item> temp = new List<item>(collection);
            List<item> sack = new List<item>();
            int sackWeight = 0;
            double sum = 0;
            while (temp.Count > 0)
            {
                sackWeight += temp[0].cost;
                if (sackWeight > capacity)
                {
                    sackWeight -= temp[0].cost;
                    break;
                }
                sack.Add(temp[0]);
                sum += temp[0].value;
                temp.RemoveAt(0);
            }
            var remain = capacity - sackWeight;
            if(temp.Count > 0)
                sum += temp[0].value * (Convert.ToDouble(remain) / temp[0].cost);
            return sum;
        }
        static public void print(List<item> collection)
        {
            var sum = 0;
            var cost = 0;
            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", "Item Name", "Item Cost", "Item Value", "Item Ratio");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            foreach (var obj in collection)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", obj.name, obj.cost, obj.value, obj.ratio);
                sum += obj.value;
                cost += obj.cost;
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Sum of Values: {0}", sum);
            Console.WriteLine("Sum of Costs: {0}\n", cost);
        }
        static int getMin(List<item> collection, int capacity)
        {
            int h = HVal(collection, capacity);
            int l = LCost(collection, capacity);
            int r = HRat(collection, capacity);
            return (h > l ? h : l) > r ? (h > l ? h : l) : r;
        }

        //Exhaustive Search
        static Sack exhaustive(List<item> collection, int cap, Stopwatch time)
        {
            time.Start();
            Sack max = new Sack(cap);
            Stack<Sack> q = new Stack<Sack>();
            q.Push(max);
            while (q.Count > 0)
            {
                //create and add new sacks to Queue
                Sack temp = q.Pop();
                List<item> next;
                if (temp.getSack() != null)
                    next = new List<item>(temp.getSack());
                else
                    next = new List<item>();
                if (temp.getCount() < collection.Count)
                {
                    Sack notTake = new Sack(next, cap, temp.getCount() + 1);
                    next.Add(collection[temp.getCount()]);
                    if (next.Add(collection[temp.getCount()]).name == "A" && temp.getSack() != null)
                        temp.displayCon();
                    Sack take = new Sack(next, cap, temp.getCount() + 1);
                    q.Push(take);
                    q.Push(notTake);
                }

                //check if temp is greater than sack
                if (temp.getVal() > max.getVal() && temp.getCost() <= cap)
                {
                    max = new Sack(temp);
                }
                if (time.ElapsedMilliseconds > 600000)
                    break;

            }
            time.Stop();
            return max;
        }

        //Phase 3 Optimizations
        static Sack baseOptimize(List<item> collection, int cap, int greedy, Stopwatch time)
        {
            time.Start();
            Sack max = new Sack(cap);
            Stack<Sack> q = new Stack<Sack>();
            int min = greedy;
            q.Push(max);
            while (q.Count > 0)
            {
                //create and add new sacks to Queue
                Sack temp = q.Pop();
                List<item> next;
                if (temp.getSack() != null)
                    next = new List<item>(temp.getSack());
                else
                    next = new List<item>();
                if (temp.getCount() < collection.Count)
                {
                    Sack notTake = new Sack(next, cap, temp.getCount() + 1);
                    //prune right subtree if it is over capacity
                    if (temp.getCost() + collection[temp.getCount()].cost <= cap)
                    {
                        next.Add(collection[temp.getCount()]);
                        Sack take = new Sack(next, cap, temp.getCount() + 1);
                        q.Push(take);
                    }

                    //prune left subtree if all possible items are less than current min
                    int valLeft = 0;
                    for (int i = temp.getCount(); i < collection.Count; i++)
                    {
                        valLeft += collection[i].value;
                    }
                    if (notTake.getVal() + valLeft >= min)
                    {
                        q.Push(notTake);
                    }
                }

                //check if temp is greater than sack
                if (temp.getVal() > max.getVal())
                {
                    max = new Sack(temp);
                    if (max.getVal() > min)
                        min = max.getVal();
                }
                if (time.ElapsedMilliseconds > 600000)
                    break;
            }
            time.Stop();
            return max;
        }

        //Threaded Optimizations
        static Sack baseOp(List<item> collection, int cap, int greedy, Sack pass, Stopwatch time)
        {
            Sack max = new Sack(pass);
            Stack<Sack> q = new Stack<Sack>();
            int min = greedy;
            q.Push(max);
            while (q.Count > 0)
            {
                //create and add new sacks to Queue
                Sack temp = q.Pop();
                List<item> next;
                if (temp.getSack() != null)
                    next = new List<item>(temp.getSack());
                else
                    next = new List<item>();
                if (temp.getCount() < collection.Count)
                {
                    Sack notTake = new Sack(next, cap, temp.getCount() + 1);

                    //prune right subtree if it is over capacity
                    if (temp.getCost() + collection[temp.getCount()].cost <= cap)
                    {
                        next.Add(collection[temp.getCount()]);
                        Sack take = new Sack(next, cap, temp.getCount() + 1);
                        q.Push(take);
                    }

                    //prune left subtree if all possible items are less than current min
                    int valLeft = 0;
                    for (int i = temp.getCount(); i < collection.Count; i++)
                    {
                        valLeft += collection[i].value;
                    }
                    if (notTake.getVal() + valLeft >= min)
                    {
                        q.Push(notTake);
                    }
                }

                //check if temp is greater than sack
                if (temp.getVal() > max.getVal() && temp.getCost() <= cap)
                {
                    max = new Sack(temp);
                    if (max.getVal() > min)
                        min = max.getVal();
                }
                if (time.ElapsedMilliseconds > 600000)
                    break;
            }
            return max;
        }
        static Sack getMax(List<Sack> bag)
        {
            Sack max = new Sack(0);
            foreach (var obj in bag)
            {
                if (obj.getVal() > max.getVal())
                    max = obj;
            }
            return max;
        }
        static Sack threadOp(List<item> collection, int cap, int greedy, Stopwatch time)
        {
            time.Start();

            //initialize sacks to pass into function
            List<item> empty;
            empty = new List<item>();
            Sack left1 = new Sack(empty, cap, 2);
            empty.Add(collection[0]);
            Sack left2 = new Sack(empty, cap, 2);
            empty.Clear();
            empty.Add(collection[1]);
            Sack right1 = new Sack(empty, cap, 2);
            empty.Add(collection[0]);
            Sack right2 = new Sack(empty, cap, 2);

            //create sacks empty sacks and their corresponding threads
            Task<Sack> one = Task<Sack>.Factory.StartNew(() => { return baseOp(collection, cap, greedy, left1, time); });
            Task<Sack> two = Task<Sack>.Factory.StartNew(() => { return baseOp(collection, cap, greedy, left2, time); });
            Task<Sack> three = Task<Sack>.Factory.StartNew(() => { return baseOp(collection, cap, greedy, right1, time); });
            Task<Sack> four = Task<Sack>.Factory.StartNew(() => { return baseOp(collection, cap, greedy, right2, time); });

            //get the largest value sack of the four and return it
            List<Sack> bag = new List<Sack>();
            bag.Add(one.Result);
            bag.Add(two.Result);
            bag.Add(three.Result);
            bag.Add(four.Result);

            time.Stop();
            return getMax(bag);
        }

        static void Main(string[] args)
        {
            //Get file name and create filestream
            Console.Write("Input filename: ");
            var filename = Console.ReadLine();
            StreamReader file = new StreamReader(filename);

            //create the item list
            List<item> collection = new List<item>();
            var capacity = Convert.ToInt32(file.ReadLine());
            var newItem = file.ReadLine();
            while (newItem != null)
            {
                var thing = newItem.Split(',');
                item temp;
                temp.name = thing[0];
                temp.cost = Convert.ToInt32(thing[1]);
                temp.value = Convert.ToInt32(thing[2]);
                temp.ratio = Convert.ToDouble(temp.value) / Convert.ToDouble(temp.cost);
                collection.Add(temp);
                newItem = file.ReadLine();
            }
            
            //run computations
            int min = getMin(collection, capacity);
            double max = Partial(collection, capacity);
            Stopwatch threadTime = new Stopwatch();
            Stopwatch baseTime = new Stopwatch();
            Stopwatch exhaustiveTime = new Stopwatch();
            Sack thread = threadOp(collection, capacity, min, threadTime);
            Sack prune = baseOptimize(collection, capacity, min, baseTime);
            Sack dumb = exhaustive(collection, capacity, exhaustiveTime);

            //get output filename
            Console.Write("Enter Output Filename without .txt: ");
            var t = Console.ReadLine();
            var text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            text += "\\" + t + ".txt";
            StreamWriter output = new StreamWriter(@text);
            output.AutoFlush = true;
            output.WriteLine("File read in: {0}", filename);
            output.WriteLine("Capacity: {0}", capacity);
            output.WriteLine("Greedy Min Boundary: {0}", min);
            output.WriteLine("Greedy Max Boundary: {0}", max);
            output.WriteLine("");
            output.WriteLine("Exhaustive Solution: ");
            dumb.display(output);
            output.WriteLine("Time Taken: {0}", exhaustiveTime.Elapsed);
            output.WriteLine();
            output.WriteLine("Base Optimizations: ");
            prune.display(output);
            output.WriteLine("Time Taken: {0}", baseTime.Elapsed);
            output.WriteLine();
            output.WriteLine("Personal Optimization: ");
            thread.display(output);
            output.WriteLine("Time Taken: {0}", threadTime.Elapsed);
            Console.WriteLine("Output file at: {0}", text);
        }
    }
}
