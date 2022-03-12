using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDrawTest
{
    public class RandomDraw
    {
        /// <summary>
        /// 把抽奖概率精准到0.01%
        /// </summary>
        public const uint Parameters = 100;
        private readonly List<string> _name = new List<string>();
        private readonly List<uint> _probability = new List<uint>();
        private static readonly Random _random = new Random();       

        public void AddPrize(string name, decimal probability)
        {
            if (probability <= 0.009999999M)
            {
                throw new ArgumentOutOfRangeException();
            }
            _name.Add(name);
            _probability.Add((uint)(probability*Parameters));
        }

        public string GetRandomResult()
        {
            var result = GetResult(_probability);
            return _name[result];
        }

        private static int GetResult(List<uint> array)
        {
            uint totalNumber = 0;
            foreach (uint j in array)
            {
                //获取总数
                totalNumber += j;
            }
            //Console.WriteLine($"总获奖概率={totalNumber}");
            double random;
            for (var i = 0; i < array.Count; ++i)
            {
                //获取 0-总数 之间的一个随随机整数
                random = _random.NextDouble() * totalNumber;
                if (random < array[i])
                {
                    //如果在当前的概率范围内,得到的就是当前概率
                    return i;
                }
                else
                {
                    //否则减去当前的概率范围,进入下一轮循环
                    totalNumber -= array[i];
                }
            }
            return 0;
        }
    }
}
